using System;
using System.Collections.Generic;
using System.Text;

namespace CLScript
{
    public class CLS_Content
    {
        public CLS_Environment environment
        {
            get;
            private set;
        }
        public CLS_Content(CLS_Environment environment)
        {
            this.environment = environment;
        }
        public class Value
        {
            public Type type;
            public object value;
            public int breakBlock = 0;//是否是块结束
            public static Value FromICLS_Value(ICLS_Value value)
            {
                Value v = new Value();
                v.type = value.type;
                v.value = value.value;
                return v;
            }
            public static Value One
            {
                get
                {
                    if(g_one==null)
                    {
                        g_one = new Value();
                        g_one.type = typeof(int);
                        g_one.value = (int)1;
                    }
                    return g_one;
                }
            }
            public static Value OneMinus
            {
                get
                {
                    if (g_oneM == null)
                    {
                        g_oneM = new Value();
                        g_oneM.type = typeof(int);
                        g_oneM.value = (int)-1;
                    }
                    return g_oneM;
                }
            }
            public static Value Void
            {
                get
                {
                    if (g_void == null)
                    {
                        g_void = new Value();
                        g_void.type = typeof(void);
                        g_void.value = null;
                    }
                    return g_void;
                }
            }
            static Value g_one = null;
            static Value g_oneM = null;
            static Value g_void = null;
        }

        public Dictionary<string, Value> values = new Dictionary<string, Value>();
        public void Define(string name,Type type)
        {
            if (values.ContainsKey(name)) throw new Exception("已经定义过");
            Value v = new Value();
            v.type = type;
            values[name] = v;
        }
        public void Set(string name,object value)
        {
            if (!values.ContainsKey(name)) throw new Exception("值没有定义过");
            if (values[name].type == typeof(CLS_Type_Var.var)&&value!=null)
                values[name].type = value.GetType();
            values[name].value = value;
        }

        public void DefineAndSet(string name,Type type,object value)
        {
            if (values.ContainsKey(name)) throw new Exception("已经定义过");
            Value v = new Value();
            v.type = type;
            v.value = value;
            values[name] = v;
        }
        public Value Get(string name)
        {
            if (!values.ContainsKey(name)) throw new Exception("值没有定义过");
            return values[name];
        }
    }
}
