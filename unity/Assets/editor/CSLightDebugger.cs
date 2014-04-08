using UnityEngine;
using System.Collections;
using UnityEditor;

public class CSLightDebugger
{
    public class Monoiter
    {

    }
    static Monoiter mm = new Monoiter();
    [MenuItem("C#Light/ShowDebug")]

    public static void ShowDebug()
    {

        if(debugger==null)
        {
            debugger = CreateFormAss("../libgen/CSLightDebuger.dll");
            debugger.Init( logger ,mm);
        }
        debugger.Show();

    }
    public static CSLight.ICLS_Debugger debugger
    {
        get;
        private set;
    }
    static CSLight.ICLS_Debugger CreateFormAss(string path)
    {
        string p = System.IO.Path.GetFullPath(path);
        Debug.Log("p=" + p);
        var ass = System.Reflection.Assembly.LoadFile(p);
        var t = ass.GetType("CSLightDebug.Debugger");
        return t.Assembly.CreateInstance(t.FullName) as CSLight.ICLS_Debugger;

    }
    static  Logge logger = new Logge();
    public class Logge : CSLight.ICLS_Logger
    {
        public void Log(string str)
        {
            Debug.Log(str);
        }

        public void Log_Error(string str)
        {
            Debug.LogError(str);
        }

        public void Log_Warn(string str)
        {
            Debug.LogWarning(str);
        }
    }
}
