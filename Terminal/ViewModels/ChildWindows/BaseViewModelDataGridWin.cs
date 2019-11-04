using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TradingLib;




namespace Terminal.ViewModels.ChildWindows
{



    public class BaseViewModelDataGridWin <TDataLogUpdate, TDataLogEl, TControl> : BaseViewModelChildWin
                                                           where TControl : UIElement, IDataControl
                                                           where TDataLogUpdate  : IDataLogUpdateCommand<TDataLogEl>
    {
        protected IWindowWithDatagrids<TControl> _windowDataGrid;

        //protected Dictionary<string, List<TDataLogEl>> _actualLog;

        protected List<ModelDataLog<TDataLogEl>> _lstModelActLog = new List<ModelDataLog<TDataLogEl>>();



        public BaseViewModelDataGridWin()
        {
            //_actualLog = new Dictionary<string, List<TDataLogEl>>();


        }

        /// <summary>
        /// Update model and view
        /// 
        /// Call when received new data from server
        /// </summary>
        /// <param name="data"></param>
        /// <param name="conId"></param>
        public override void UpdateData(object data, int conId)
        {
           
            try
            {
                                            
                TDataLogUpdate dataUpdate = (TDataLogUpdate)data;
                dataUpdate.Sort();

                var lstElWithConId = _lstModelActLog.FindAll (el => el.ConId == conId);
                //if data doesn't exist on recieved log but exist on
                //window do remove from window
                List<string> lstToRemove = new List<string>();
                foreach (var v in lstElWithConId)
                    if (!dataUpdate.DictLog.ContainsKey(v.Instrument))
                    {
                        //first add to list
                        lstToRemove.Add(v.Instrument);
                       
                    }
                //than iterate list and do remove
                foreach (var v in lstToRemove)
                {

                    _lstModelActLog.RemoveAll(el => el.Instrument == v && el.ConId == conId);

                    if (_windowDataGrid != null)
                    {

                        _windowDataGrid.GUIDispatcher.BeginInvoke
                            (new Action(() =>

                        _windowDataGrid.RemoveNewDataFrame(v)
                        ));

                    }


                }






                    //if not exist on window do create new one
                    foreach (var v in dataUpdate.DictLog)
                {
                    string isin = v.Key;

                    var res = _lstModelActLog.Find(el => el.Instrument == isin && el.ConId == conId);

                    //if (!_actualLog.ContainsKey(v.Key)) 
                    if(res == null)
                    {
                        //update "model"
                        //_actualLog[isin] = dataUpdate.DictLog[isin];
                        _lstModelActLog.Add(new ModelDataLog<TDataLogEl>()
                                             {
                                                ConId = conId,
                                                Instrument = isin,
                                                DataLog = dataUpdate.DictLog[isin]
                        }
                                                );
                        


                        //update view
                        if (_windowDataGrid != null)
                        {
                            _windowDataGrid.GUIDispatcher.BeginInvoke
                                (new Action(() =>
                                    _windowDataGrid.AddNewDataFrame(isin)
                            ));
                            //rebind
                            BindDataLog(isin);
                        }

                    }
                    //if recieved data differs that on window do update it

                    else if (res.DataLog.Except(dataUpdate.DictLog[isin]).Any()) 
                    {
                        ///update model
                        //_actualLog[isin] = dataUpdate.DictLog[isin];
                        res.DataLog = dataUpdate.DictLog[isin];
                        //rebind
                        BindDataLog(isin);

                    }
                }

            }
            catch (Exception e)
            {
                Error("BaseViewModelDataGridWin.UpdateData", e);

            }



        }


        protected override void CreateControls()
        {

            try
            {
                base.CreateControls();

                _windowDataGrid = (IWindowWithDatagrids<TControl>)_view;
               
               // foreach (var kvp in _actualLog)
               foreach(var el in _lstModelActLog)
                {
                    _windowDataGrid.AddNewDataFrame(el.Instrument);
                    BindDataLog(el.Instrument);

                }
            }
            catch (Exception e)
            {
                Error("BaseViewModelDataGridWin.CreateControls",e);
            }
        }



        protected void BindDataLog(string isin)
        {
            if (_windowDataGrid != null)
                _windowDataGrid.GUIDispatcher.BeginInvoke
                    (new Action(() =>
                        {
                            var res = _lstModelActLog.Find(el => el.Instrument == isin);
                            if (res != null)
                                _windowDataGrid.ViewDataUpdater.BindDataGrid(isin, res.DataLog);
                        }
                        //_windowDataGrid.ViewDataUpdater.BindDataGrid(isin, _actualLog[isin] 

                    ));
            
        }

       



    }
}
