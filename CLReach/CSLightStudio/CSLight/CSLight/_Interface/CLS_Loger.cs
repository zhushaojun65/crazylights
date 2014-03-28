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
    public interface ICLS_Debugger:ICLS_Logger
    {
        /// <summary>
        /// 设置代码集合
        /// </summary>
        /// <param name="?"></param>
        void InitCodeCollection(ICLS_CodeCollection coll);


        void DebugRun();
        void DebugPause();
        void DebugStop();
     
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