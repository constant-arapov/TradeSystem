using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using  System.Threading;

using System.Windows.Threading;
using System.Threading.Tasks;

using Common.Interfaces;


namespace Common.Collections
{
    public class CObservableIdCollection<T,P> : ObservableCollection<T> where T : IIDable <P>
    {
        
        private Mutex mx = new Mutex ();

  
        public CObservableIdCollection()
        {

         

        }


      

        
        public void UpdateWithId(IIDable <P> obj)
        {
           /* try*/
            {
                int i = 0;
                for (i = 0; i < this.Count; i++)
                {

                    if (AreEqual(this[i].Id, obj.Id))
                    {

                        this[i] = (T)obj;                       
                        return;
                    }

                }

                this.Add((T)obj);
            }
            /*catch (Exception e)
            {

                throw e;
            }
            */
            
        }
   

        public void RemoveWithId(P id)
        {

            for (int i = 0; i < this.Count; i++)
            {
                if (AreEqual(this[i].Id, id))                                                  
                    this.RemoveAt(i);                
            }

        }       

        public static bool AreEqual(P param1, P param2)

        {
                return EqualityComparer<P>.Default.Equals(param1, param2);

        }




    }
}
