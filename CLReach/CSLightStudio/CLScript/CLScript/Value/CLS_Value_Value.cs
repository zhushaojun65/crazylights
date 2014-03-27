using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{
    public class CLS_Value_Value<T> : ICLS_Value
    {
        public Type type
        {
            get { return typeof(T); }
        }


        public T value_value;
        public object value
        {
            get
            {
                return value_value;
            }
        }
        public override string ToString()
        {
            return type.Name + "|" + value_value.ToString();
        }


        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            CLS_Content.Value v = new CLS_Content.Value();
            v.type = this.type;
            v.value = this.value_value;
            return v;
        }
    }
    public class CLS_Value_Null : ICLS_Value
    {
        public Type type
        {
            get { return null; }
        }

        public string Dump()
        {
            return "<unknown> null";
        }
      
        public object value
        {
            get
            {
                return null;
            }
        }
        public override string ToString()
        {
            return "<unknown> null";
        }


        public List<ICLS_Expression> listParam
        {
            get { return null; }
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            CLS_Content.Value v = new CLS_Content.Value();
            v.type = this.type;
            v.value = null;
            return v;
        }
    }

    public class CLS_Value_Object:ICLS_Value
    {
        public CLS_Value_Object(Type type)
        {
            this.type = type;
            this.value_value = null;
        }

        public Type type
        {
            get;
            private set;
        }

        public object value_value;
        public object value
        {
            get
            {
                return value_value;
            }
        }

        public List<ICLS_Expression> listParam
        {
            get { throw new NotImplementedException(); }
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            CLS_Content.Value v = new CLS_Content.Value();

            v.type = this.type;
            v.value = this.value_value;

            return v;
        }
    }
}