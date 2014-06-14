using CSLightFrameworkDemo;
using System;
using System.Collections.Generic;
using System.Text;


    static class testcode01
    {
        public static void _new(MyType parent)
        {
            parent.script.CallScriptFuncWithoutParam("_initnoparam");
        }
        public static void _initnoparam(MyType parent)
        {
            parent.AddTextToList("_inited");
            parent.SetColorBlue();
        }
        public static void _update(MyType parent, string param)
        {
            parent.AddTextToList("_ypdate:" + param);
        }
        public static void _click01(MyType parent)
        {
            parent.AddTextToList("_click01" );
        }
        public static void _click02(MyType parent,string param)
        {
            parent.AddTextToList("_click02:"+param);
        }
        public static void _click03(MyType parent, List<string> param)
        {
            string strout = "";
            foreach(var p in param)
            {
                strout += p;
            }
            parent.AddTextToList("_click03:" + strout);
        }
    }
