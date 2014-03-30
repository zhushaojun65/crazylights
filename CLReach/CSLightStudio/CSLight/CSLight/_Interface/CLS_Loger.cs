using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{
   
    public interface ICLS_Logger
    {
        void Log(string str);
        void Log_Warn(string str);
        void Log_Error(string str);
    }
    //Logger 升级成更强大的Debugger,而logger成为Debugger的一部分功能

    //CodeCollection提供给Debugger操作脚本文件的方法
    public interface ICLS_CodeCollection
    {
        string rootPath
        {
            get;
        }
        IList<string> listCode
        {
            get;
        }
        string getCode(string name);
        bool SaveCode(string name,string code);

        bool newCode(string name);
    }
    public delegate void func();
    public interface ICLS_Debugger:ICLS_Logger
    {

        void BeginDebugThread(ICLS_Logger loggerWithoutDebug,func onDebugWinClose, ICLS_CodeCollection coll);
        /// <summary>
        /// 跳转到代码位置
        /// </summary>
        /// <param name="file"></param>
        /// <param name="code"></param>
        /// <param name="line"></param>
        /// <param name="col"></param>
        void JumpToCode(string file, string code=null,int line=0,int col=0);
    }

}