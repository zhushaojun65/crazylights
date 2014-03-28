using System;
using System.Collections.Generic;
using System.Text;

namespace CSLightDebug
{
    public class Debugger:CSLight.ICLS_Debugger
    {
        CSLight.ICLS_CodeCollection codes;
        public void InitCodeCollection(CSLight.ICLS_CodeCollection coll)
        {
            codes =coll;
        }

        public void Log(string str)
        {
            throw new NotImplementedException();
        }

        public void Log_Warn(string str)
        {
            throw new NotImplementedException();
        }

        public void Log_Error(string str)
        {
            throw new NotImplementedException();
        }



        public void DebugRun()
        {
            throw new NotImplementedException();
        }

        public void DebugPause()
        {
            throw new NotImplementedException();
        }

        public void DebugStop()
        {
            throw new NotImplementedException();
        }

        public void JumpToCode(string file, string code = null, int line = 0, int col = 0)
        {
            throw new NotImplementedException();
        }
    }
}
