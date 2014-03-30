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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerWithoutDebug"></param>
        /// <param name="moniterForAutoQuit">弱引用对象，退出监视</param>
        void Init(ICLS_Logger loggerWithoutDebug,object moniterForAutoQuit);
        void Show();
        void Hide();
        void Dispose();//终止调试器

        /// <summary>
        /// 不要影响CodeCollection的回收,CodeCollection可能是由运行时新增加的
        /// </summary>
        /// <param name="coll">弱引用的coll</param>
        void RegCodeCollection(string name, CSLight.ICLS_CodeCollection coll);
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