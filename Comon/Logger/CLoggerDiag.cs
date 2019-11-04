using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using System.Runtime.CompilerServices;

namespace Common.Logger
{
    public class CLoggerDiag : CLogger
    {
        public CLoggerDiag(string fileName, bool flushMode = true, string subDir = "", bool useMicroseconds = false)
            : base (fileName,  flushMode, subDir, useMicroseconds)
        {


        }



        [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogMeth(string message = "", int stockDepth=1)
        {
            StackFrame frame = new StackFrame(stockDepth);
            var method = frame.GetMethod();
            var Name = method.Name;

            Log(Name + " - " + message);


        }
       [MethodImpl(MethodImplOptions.NoInlining)]
        public void LogMethEntry(int stockDepth = 2)
        {
            LogMeth("Entry", stockDepth);
        }

        public void LogMethExit(int stockDepth = 2)
        {
            LogMeth("Exit", stockDepth);
        }

    }
}
