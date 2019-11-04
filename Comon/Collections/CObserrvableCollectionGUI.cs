using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections.ObjectModel;
using System.Windows.Threading;


namespace Common.Collections
{
    public class CObserrvableCollectionGUI<T> : ObservableCollection <T>
    {
        public  Dispatcher GUIDispatcher { get; set; }

        private List<T> m_listBuff = new List<T>();


        public CObserrvableCollectionGUI()
        {


        }


        public CObserrvableCollectionGUI (Dispatcher disp)
        {
            GUIDispatcher = disp;

        }
        public new void Add(T el)
        {
           

            if (GUIDispatcher!=null)
                GUIDispatcher.Invoke(new Action(() => base.Add(el)));
            
        }
        public  void PushBack(T el)
        {
            
            if (GUIDispatcher != null)
                GUIDispatcher.Invoke(new Action(() => this.Insert(0, el)));


        }

        public void AddBuffering(T el)
        {
            m_listBuff.Insert(0, el);
        }

        public void FlushBuffer()
        {
            foreach (T s in m_listBuff)
                this.Add(s);

        }
      /*  public  void Add(T el, Dispatcher disp)
        {
            UpdateDispatcher(disp);
            Add(el);

        }
        */

        public void UpdateDispatcher(Dispatcher disp)
        {
            if (GUIDispatcher==null)
                 GUIDispatcher = disp;

        }

    }
}
