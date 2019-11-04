using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MOEX.ASTS.Client;

using Common.Interfaces;

using TradingLib.Data.DB;



using ASTS.Interfaces.Interactions;
using ASTS.DealingServer.Stocks;


namespace ASTS.Tables
{
    public class CTableExt_OrderBook : CBaseTable
    {
        private List<CTableRecrd> _orderbook;
        //TODO move to CTableExt_OrderBook
        // Специальная структура для хранения "стаканов" - блоки записей,
        // хешированные по коду инструмента (SECBOARD + SECCODE).
        protected Dictionary<string, List<CTableRecrd>> _dictOrderbooks = new Dictionary<string, List<CTableRecrd>>();

		private CStockBoxASTS _stockBox;
		private CListInstruments _listInstruments;

        public CTableExt_OrderBook(string name, IDealingServerForTableExt_OrderBook client, 
                                    CStockBoxASTS stockBox, CListInstruments listInstruments)
            : base(name, client)
        {
			_stockBox = stockBox;
			_listInstruments = listInstruments;

            client.IsStockOnline = true;
            client.EvStockOnline.Set();

        }

        
    



		/// <summary>
		/// Специфическая операция для таблиц типа "котировки" (table.isOrderbook())
		/// Для таких таблиц значения ключевых полей setKeyField() не задаются для 
		/// каждой записи, вместо этого идет "переключение стакана" - блока записей.
		/// Данный вызов информирует вас о том, что начинается новый блок
		/// для указанного инструмента ticker.
		/// </summary>		
        public override void  SwitchOrderbook(Meta.Message source, string filter, string board, string paper)
        {

			
			//New orderbook block (for another instrument) recieved. 
			//So, update previous orderbook block (instrument) before
			//orderbook object will clear or replaced
			if (_orderbook != null)
			{
				_stockBox.Update(_orderbook);
			}



            if (_dictOrderbooks.TryGetValue(board + '.' + paper, out _orderbook))
            {
                // "Стакан" уже есть - его нужно очистить,
                // т.к. новые значения полностью заменяют старые.
                _orderbook.Clear();
            }
            else
            {
                // Инструмент встретился впервые,
                // подготовим для него "стакан"
                _orderbook = new List<CTableRecrd>();
                _dictOrderbooks.Add(board + '.' + paper, _orderbook);
            }
        }





        public override bool InitRecordUpdate(Meta.Message source, string filter)
        {
            if (source.IsOrderbook)
            {
                // Для таблиц типа "orderbook" - специальная обработка.
                // Запись всегда будет новой, добавим ее в "стакан"
				if (_orderbook != null)
				{
					_currentRecord = new CTableRecrd();
					_orderbook.Add(_currentRecord);
				}
                return true;
            }
            return false;

        }

        public override void DoneTableUpdate(Meta.Message source, string filter)
        {

			// Просто окончание работы парсера, а-ля commit.
			// Очистка более ненужных переменных.
			if (_orderbook != null)
			{

				_stockBox.Update(_orderbook);


			}
       
			_orderbook = null;
        }

    }
}
