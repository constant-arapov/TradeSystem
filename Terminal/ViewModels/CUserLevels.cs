using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;




using Common;
using Common.Utils;

using Terminal.Common;

namespace Terminal.ViewModels
{
    public class CUserLevels
    {

        private List<CLevelEl> _lstLevels = new List<CLevelEl>();
        private object _lck = new object();
        //private bool _bNeedSave = false;


        private CIOBinary<List<CLevelEl>> _levelsIO;

        private AutoResetEvent _evSave = new AutoResetEvent(false);
        private AutoResetEvent _evDataLoaded = new AutoResetEvent(false);


        private string _instrument;
        
        
        public CUserLevels(string instrument)
        {
            _instrument = instrument;
            _levelsIO = new CIOBinary<List<CLevelEl>>(instrument, "Levels");
            CUtil.TaskStart(TaskLoadData);
            CUtil.TaskStart(TaskSaver);
           
            //_levelsIO = new CIOBinary<List<CLevelEl>>(


            
        }
        private void TaskLoadData()
        {
            try
            {
                _lstLevels = _levelsIO.LoadData();

            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("UserLevels TaskLoadData", e);
            }
            finally
            {
                _evDataLoaded.Set();
            }

        }


        public void AddDelUserLevel(double price)
        {
            lock (_lck)
            {
                int i=0;
                bool bFound = false;
                for (i = 0; i < _lstLevels.Count; i++)
                    if (_lstLevels[i].Price == price)
                    {
                        bFound = true;
                        break;
                    }


                if (bFound)
                    _lstLevels.RemoveAt(i);
                else
                    _lstLevels.Add(new CLevelEl { Dt = DateTime.Now, Price = price });

                _evSave.Set();
            }
        }

        public List<CLevelEl> GetCopy()
        {

            List<CLevelEl> lst = new List<CLevelEl>();

            lock (_lck)
            {
                foreach (var el in _lstLevels)
                    lst.Add(el);

            }

            return lst;
        }

        private void TaskSaver()
        {
            try
            {

                _evDataLoaded.WaitOne();


                while (true)
                {
                    _evSave.WaitOne();
                    
                     List<CLevelEl> lst = GetCopy();
                     if (lst != null)
                     {
                         _levelsIO.WriteToFile(lst);
                     }


                    
                   // Thread.Sleep(1000);                   
                }
            }
            catch (Exception e)
            {
                CKernelTerminal.ErrorStatic("CUserLevels.TaskSaver", e);

            }


        }




    }
}
