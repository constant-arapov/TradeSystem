using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


using TradingLib;
using TradingLib.ProtoTradingStructs;



namespace Terminal.ViewModels.ChildWindows
{
    public class ViewModelPosLog<TDataLogUpdate, TDataLog, TControl> : BaseViewModelDataGridWin<TDataLogUpdate, TDataLog, TControl>
                                                                where TControl : UIElement, IDataControl
                                                                where TDataLogUpdate : IDataLogUpdateCommand<TDataLog>
    {


        public override void UpdateData(object data, int conId)
        {
            //for normal PosLog update, by default do use base class method
            if (data is TDataLogUpdate)
                base.UpdateData(data, conId);
            //for "late" fee update (for bitfinex) use specific algo
            else if (data is CUserPosLogUpdLate)
            {
                CUserPosLogUpdLate userPosLogUpd = (CUserPosLogUpdLate)data;
                var res = _lstModelActLog.Find(el => el.Instrument == userPosLogUpd.Instrument && el.ConId == conId);
                
                if(res!=null)
                    if(res.DataLog.Count>0)
                //if (_actualLog.ContainsKey(userPosLogUpd.Instrument))
                  //  if (_actualLog[userPosLogUpd.Instrument].Count>0)
                    {
                        if (_windowDataGrid != null)//non-opened window
                        {
                            _windowDataGrid.GUIDispatcher.Invoke
                                        (new Action(() =>
                                        {

                                            LateUpdPoslog(userPosLogUpd);
                                        }
                                        ));
                        }
                        else//opened window
                        {
                            LateUpdPoslog(userPosLogUpd);
                        }


                    BindDataLog(userPosLogUpd.Instrument);
                }

            }
           




        }

        private void LateUpdPoslog(CUserPosLogUpdLate userPosLogUpd)
        {
            //object el = _actualLog[userPosLogUpd.Instrument].First();
            object res = _lstModelActLog.Find(el => el.Instrument == userPosLogUpd.Instrument);
            if (res != null)
            {
                
            
                    //tempo for debug, remove
                    try
                    {
                        if (res is ModelDataLog<CUserPosLog>)
                        {                        
                            ModelDataLog<CUserPosLog> upl = (ModelDataLog<CUserPosLog>)res;
                            CUserPosLog   userPoslogAct = upl.DataLog.First();

                            userPoslogAct.Fee = userPosLogUpd.Fee;
                            userPoslogAct.VMClosed_RUB = userPosLogUpd.VMClosed_RUB;
                        }
                    }
                    catch (Exception e)
                    {
                        System.Threading.Thread.Sleep(0);
                        throw e;
                    }

               
            }
        }






    }
}
