using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Common;
using Common.Interfaces;
using Common.Logger;

using TradingLib.Interfaces.Interaction;

using MOEX.ASTS.Client;

using ASTS.Interfaces;
using ASTS.Interfaces.Interactions;

namespace ASTS.Tables
{
       // Very simple table.
    public abstract class CBaseTable : CBaseFunctional,   ITarget
    {

        // Записи, хэшированные по значению первичного ключа (если есть)
        // или по порядковому номеру при отсутствии ключевых полей.
        readonly Dictionary<string, CTableRecrd> _records = new Dictionary<string, CTableRecrd>();


		public  bool IsNullDecimal { get; set; }



        private OrderedDictionary _keys = new OrderedDictionary();

        protected CTableRecrd _currentRecord;


		public static CBaseTable CreateTable(string name, IDealingServerForASTSConnector dealingServer)
		{

            if ("MARKETS" == name)
                return new CTableMarkets(name, dealingServer);
            else if ("TESYSTIME" == name)
                return new CTableSysTime(name, dealingServer, dealingServer.GUIBox);
            else if ("SECURITIES" == name)
                return new CTableSecurities(name, dealingServer, dealingServer.Instruments, dealingServer.StockExchId);
            else if ("ALL_TRADES" == name)
                return new CTableAll_Trades(name, dealingServer);
            else if ("ORDERS" == name)
                return new CTableOrders(name, dealingServer, dealingServer.UserOrderBoxInp);
            else if ("TRADES" == name)
                return new CTableTrades(name, dealingServer);
            else if ("EXT_ORDERBOOK" == name)
                return new CTableExt_OrderBook(name, dealingServer, dealingServer.StockBoxInp, dealingServer.Instruments);
            else if ("RM_HOLD" == name)
                return new CTableRm_Holds(name, dealingServer);
            else if ("TRADETIME" == name)
                return new CTableTradeTimes(name, dealingServer, dealingServer.SessionBoxInp);
            else if ("TRDTIMETYPES" == name)
                return new CTableTrdTimeTypes(name, dealingServer, dealingServer.SessionBoxInp);
            else if ("STATS" == name)
                return new CTable_Stats(name, dealingServer, dealingServer.SessionBoxInp);
            else if ("POSITIONS" == name)
                return new CTablePositions(name, dealingServer);
            else
                throw new ApplicationException("CreateTable. Unknown table");

			//return new CBaseTable(name, _alarmer);
		}




        #region ITarget implementation

                                                                                         //TODO: swith off flushMode
        public CBaseTable(string name, IAlarmable alarmer) 
            : base (name, alarmer, new CLogger(name, flushMode:true, subDir:"Tables"))            
        {



        }

   

        public void InitTableUpdate(Meta.Message source, string filter)
        {
            // Парсер начинает обрабатывать буфер ответа.
            // Здесь при необходимости нужно провести
            // обработку в зависимости от комбинации значений
            // source.ClearOnUpdate, source.IsOrderbook
            if (source.ClearOnUpdate)
                _records.Clear();
        }

       

        public virtual void DoneTableUpdate(Meta.Message source, string filter)
        {
          
        }

      

        public void SetKeyValue(Meta.Field field, object value)
        {
            // Задает значения ключевых полей.
            // В демо-примере - собираем значения в dictionary.
            // По окончании обработки записи собранные значения 
            // следует сбросить (см. DoneRecordUpdate())
            _keys.Add(field.Name, value);
        }

        public virtual bool InitRecordUpdate(Meta.Message source, string filter)
        {

            //Log("InitRecordUpdate Name=" + source.Name);

            // Начало обработки записи.
            // Здесь нужно произвести поиск по ключам
            // и вернуть true, если запись не найдена (т.е. новая)

          

            if (_keys.Count == 0)
            {
                // SetKeyValue() ни разу не был вызван -
                // данная таблица не имеет первичных ключей.
                // Все записи будут считаться новыми, хранить
                // их будем по порядковому номеру.
                _currentRecord = new CTableRecrd();
                _records.Add(_records.Count.ToString(), _currentRecord);
                return true;
            }
            else
            {
                // У таблицы есть первичные ключи - 
                // ищем и храним записи по их значению.
                var b = new StringBuilder();
                foreach (DictionaryEntry e in _keys)
                {
                    

                    b.Append(e.Key.ToString())
                        .Append('=');

                    if (e.Value != null)
                        b.Append(e.Value.ToString());
                   // else
                        b.Append(';');
                }
                var s = b.ToString();
                if (_records.TryGetValue(s, out _currentRecord))
                {
                    // Запись найдена
                    return false;
                }
                // Запись по ключу не найдена - создадим новую
                _currentRecord = new CTableRecrd();
                _records.Add(s, _currentRecord);
                return true;
            }
        }


        public int GetRecordDecimals()
        {
            // Парсеру для работы понадобилось знать
            // о кол-ве десятичных знаков по найденной 
            // ранее записи - отдать сохраненное.
            return _currentRecord.decimals;
        }

        public void SetRecordDecimals(int decimals)
        {
            // Парсер дли новой записи определил 
            // кол-во десятичных знаков - сохранить (в записи).
            _currentRecord.decimals = decimals;
        }

        public void SetFieldValue(Meta.Field field, object value)
        {
            // Парсер задает значение конкретного поля записи
            _currentRecord.values[field.Name] = value;
        }

        public void DoneRecordUpdate(Meta.Message source, string filter)
        {
            // Закончили обрабатывать запись.
            // Хорошее место для сохранения накопленных данных куда-то.
            // В демо-примере - просто печать в консоль.

            ProcessRecord();
            // Здесь также следует сбросить собранные значения первичных ключей,
            // чтобы подготовиться к получению значений для следующей записи.
            _keys.Clear();
            _currentRecord = null;
        }

        //for compatiable with ITarget
        public virtual void SwitchOrderbook(Meta.Message source, string filter, string board, string paper)
        {
        }


        protected virtual void ProcessRecord()
        {
            var stValues = new StringBuilder();
            foreach (var e in _currentRecord.values)
                stValues.Append(e.Key).Append('=').Append(Convert.ToString(e.Value)).Append(';');
            //Debug.WriteLine("Table: " + source.Name + "; record: " + b.ToString());
            //Console.WriteLine("Table: " + source.Name + "; record: " + b.ToString());
            Log(stValues.ToString());



        }

        #endregion
    }


    







}
