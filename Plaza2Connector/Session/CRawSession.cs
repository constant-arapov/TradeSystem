using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plaza2Connector
{
    public class CRawSession
    {

        public CRawSession(FUTINFO.session sess )
        {
            ReplId = sess.replID;
            ReplRev = sess.replRev;
            Sess_id = sess.sess_id;
            State =  sess.state;
            Begin = sess.begin;
            End = sess.end;
         
            Cl_begin = sess.inter_cl_begin;
            Cl_end = sess.inter_cl_end;

            Cl_state = sess.inter_cl_state;
            Eve_on = sess.eve_on;
            Eve_begin = sess.eve_begin;
            Eve_end = sess.eve_end;

            Mon_on = sess.mon_on;
            Mon_begin = sess.mon_begin;
            Mon_end = sess.mon_end;
            
            

        }

        public long ReplId;
        public long ReplRev;
        public int Sess_id;
        public int State;
        public DateTime Begin;
        public DateTime End;
   
        public DateTime Cl_begin;
        public DateTime Cl_end;
        public int Cl_state;
        public int Eve_on;
        public DateTime Eve_begin;
        public DateTime Eve_end;
        public int Mon_on;
        public DateTime Mon_begin;
        public DateTime Mon_end;






    }
}
