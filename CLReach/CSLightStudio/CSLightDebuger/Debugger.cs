using System;
using System.Collections.Generic;
using System.Text;

namespace CSLightDebug
{
    public class Debugger:CSLight.ICLS_Debugger
    {
        public delegate void Action();
        static WhatAFuck WindowShow =null;
        public static void OpenDebugWin(Action onClose)
        {
            if (WindowShow != null) return;

            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                WindowShow = new WhatAFuck();
                System.Windows.Forms.Application.Run(WindowShow);
                WindowShow = null;
                onClose();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
        }


        CSLight.ICLS_CodeCollection codes;
        public void InitCodeCollection(CSLight.ICLS_CodeCollection coll)
        {
            codes =coll;
        }
        
        public void Log(string str)
        {
            if(WindowShow!=null)
            WindowShow.SafeLog(str);
        }

        public void Log_Warn(string str)
        {
            WindowShow.SafeLog_Warn(str);
        }

        public void Log_Error(string str)
        {
            WindowShow.SafeLog_Error(str);
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
