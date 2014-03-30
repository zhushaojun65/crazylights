using System;
using System.Collections.Generic;
using System.Text;

namespace CSLightDebug
{
    public class Debugger:CSLight.ICLS_Debugger
    {




        CSLight.ICLS_Logger loggerWithoutDebug;
        public void Log(string str)
        {
            if (getLockedDebugTag())
            {
                WindowShow.SafeLog(str);
            }
            else
            {
                loggerWithoutDebug.Log(str);
            }
        }

        public void Log_Warn(string str)
        {
            if (getLockedDebugTag())
            {
                WindowShow.SafeLog_Warn(str);
            }
            else
            {
                loggerWithoutDebug.Log_Warn(str);
            }
        }

        public void Log_Error(string str)
        {
            if (getLockedDebugTag())
            {
                WindowShow.SafeLog_Error(str);
            }
            else
            {
                loggerWithoutDebug.Log_Error(str);
            }
        }
        static bool DebugTag = false;
        public class _debugtag
        {
            int tag = 0;
        }
        static _debugtag _debugtaglock = new _debugtag();
        public static bool getLockedDebugTag()
        {
           lock(_debugtaglock)
           {
               return DebugTag;
           }

        }
        public static void beginSetLockedDebugTag(bool tag)
        {
            System.Threading.Monitor.Enter(_debugtaglock);
            DebugTag = tag;

        }
        public static void endSetLockedDebugTag()
        {
            System.Threading.Monitor.Exit(_debugtaglock);
        }
        CSLight.ICLS_CodeCollection codes;
        MainDebugWin WindowShow;
        public void BeginDebugThread(CSLight.ICLS_Logger loggerWithoutDebug, CSLight.func onDebugWinClose, CSLight.ICLS_CodeCollection coll)
        {
            this.loggerWithoutDebug = loggerWithoutDebug;
            this.codes = coll;
            if (getLockedDebugTag())
            {
                WindowShow.SafeShow();
                return;
            }


            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                beginSetLockedDebugTag(true);
                WindowShow = new MainDebugWin();
                System.Windows.Forms.Application.Run(WindowShow);
                WindowShow = null;
                if (onDebugWinClose != null)
                    onDebugWinClose(); 
                
                endSetLockedDebugTag();
            });
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
        }
        public void JumpToCode(string file, string code = null, int line = 0, int col = 0)
        {

        }


    }
}
