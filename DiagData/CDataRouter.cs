using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;

using DBCommunicator;
using DBCommunicator.Interfaces;

    





namespace DiagData
{
    public class CDataRouter : IAlarmable, IClientDatabaseConnector
    {


        private CExceler _exceler;

        private CMySQLConnector _mysqlConnector;

        public bool IsDatabaseConnected { get; set; }
        public bool IsDatabaseReadyForOperations { get; set; }
        List<CPosLogData> _lstPoslogDat;

        public CDataRouter()
        {
            _exceler = new CExceler();
           

        }

        public void Execute()
        {
            //  _exceler.Execute();
            try
            {


                _mysqlConnector = new CMySQLConnector(host: "81.177.142.127",
                                                      database: "atfs_crypto",
                                                      user: "root",
                                                      password: "profinvest",
                                                      alarmer: this,
                                                      dbConnectorClient: this);


                _mysqlConnector.Connect();



                _lstPoslogDat = SelectPoslogData();
                DecoratePoslogData();
              



                _exceler.Execute(_lstPoslogDat);


            }
            catch (Exception e)
            {

            }
            finally
            {
                if (_mysqlConnector != null)
                    _mysqlConnector.Disconnect();
            }

        }




        private List<CPosLogData> SelectPoslogData()
        {
            string sql = String.Format(
           @"select botId, instrument, CloseAmount, PosDir, tbl.DtOpen P_DtOpen, tbl.DtClose P_DtClose, PriceOpen, PriceClose," +
            "VMClosed_Points, VMClosed_Steps, VMClosed_RUB_clean,VMClosed_RUB_user," +
            "VMClosed_RUB, P_Fee, P_FeeStock, P_FeeDealing," +
            "DealId,Moment, Amount,  DealDir,Price, D_Fee, D_FeeStock, D_FeeDealing, " +
            "OrderId,FeeBfx, FeeBfx + D_FeeStock dltFee, ExecAmount, ExecPrice,IsFeeLateCalced " +

            "from " +

           "(SELECT poslog.account_trade_Id botId, BP_DtOpen_timestamp_ms, CloseAmount, DtOpen, DtClose, PriceOpen, PriceClose, " +
           "poslog.BuySell PosDir, VMClosed_Points, VMClosed_Steps, VMClosed_RUB_clean, VMClosed_RUB_user, VMClosed_RUB, " +
           "poslog.Fee P_Fee, poslog.Fee_Stock P_FeeStock, poslog.FeeDealing P_FeeDealing, poslog.instrument instrument, " +
           "DtOpen_timestamp_ms, " +
           "userdealslog.DealId, Moment, Amount, Price, userdealslog.BuySell DealDir, userdealslog.Fee D_Fee, userdealslog.Fee_Stock D_FeeStock, userdealslog.FeeDealing D_FeeDealing, " +
           "OrderId, bfx_trades_history.Fee FeeBfx, ExecAmount, ExecPrice, poslog.IsFeeLateCalced " +

           "FROM userdealslog, poslog, bfx_trades_history where userdealslog.stock_exch_id = 4 " +
           "and userdealslog.account_trade_Id = poslog.account_trade_Id  and userdealslog.DealId = bfx_trades_history.id) tbl " +
           "where " +
           "DtOpen_timestamp_ms = BP_DtOpen_timestamp_ms " +
           "and " +
           "DtOpen >= '2018-09-13 05:45' and DtClose<'2018-09-13 23:11' " +

           "order by DtClose, Moment "


            );



            List <CPosLogData> _lst = _mysqlConnector.ExecuteSelectObject<CPosLogData>(sql);

            return _lst;
        }

        private void DecoratePoslogData()
        {
            int group = 0;
            //_lstPoslogDat[0].PosGroup = group;

            for (int i = 0; i < _lstPoslogDat.Count; i++)
            {
                if (_lstPoslogDat[i].PosGroup == 0)
                {
                    _lstPoslogDat[i].PosGroup = ++group;
                }
                else
                    continue;

                for (int j = i + 1; j < _lstPoslogDat.Count; j++)
                {
                    if (_lstPoslogDat[j].PosGroup != 0)
                        continue;

                    if (_lstPoslogDat[i].botId == _lstPoslogDat[j].botId &&
                        _lstPoslogDat[i].instrument == _lstPoslogDat[j].instrument &&
                        _lstPoslogDat[i].P_DtOpen == _lstPoslogDat[j].P_DtOpen &&
                        _lstPoslogDat[i].P_DtClose == _lstPoslogDat[j].P_DtClose)
                        _lstPoslogDat[j].PosGroup = group;


                }

            }

            for (int i=1;i<=group;i++)
            {
                var allInGroup =_lstPoslogDat.FindAll(el => el.PosGroup == i);
                var indLastInGroup = _lstPoslogDat.FindLastIndex(el => el.PosGroup == i);

                if (allInGroup != null)
                {
                    decimal sumFeeBfx = 0;
                    decimal blncBS = 0;
                    

                    allInGroup.ForEach(el2 =>
                           {
                               sumFeeBfx += el2.FeeBfx;
                               blncBS = el2.DealDir == "Buy" ? blncBS + el2.Amount : blncBS - el2.Amount;
                           }
                            );

                    _lstPoslogDat[indLastInGroup].SumBfxFee = sumFeeBfx;
                    _lstPoslogDat[indLastInGroup].DltBfxPosFee = sumFeeBfx + _lstPoslogDat[indLastInGroup].P_FeeStock;
                    _lstPoslogDat[indLastInGroup].BlnceBS = blncBS;


                }

                


            }




        }



        public void Error(string msg, Exception e=null)
        {
            throw e;

        }









    }
}
