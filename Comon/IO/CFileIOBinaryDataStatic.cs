using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Interfaces;
using Common.Utils;

namespace Common.IO
{
    public class CFileIOBinaryDataStatic : CFileIOBinary
    {


        public CFileIOBinaryDataStatic(string fileName, IAlarmable alarmer ) 
            : base (fileName,alarmer)
        {
               
        }

        protected override string GetFileName()
        {
            return _fileName;
                

        }




    }
}
