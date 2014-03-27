using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_Define : ICLS_Expression
    {
        public CLS_Expression_Define()
        {
            //listParam = new List<ICLS_Value>();
        }
        //Block的参数 一个就是一行，顺序执行，没有
        List<ICLS_Expression> _listParam = null;
        public List<ICLS_Expression> listParam
        {
            get
            {
                if (_listParam == null)
                {
                    _listParam = new List<ICLS_Expression>();
                }
                return _listParam;
            }
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {

            if (_listParam != null && _listParam.Count > 0)
            {
                CLS_Content.Value v = _listParam[0].ComputeValue(content);
                object val = v.value;
                if (value_type==typeof(CLS_Type_Var.var))
                {
                    if(v.type!=null)
                        value_type = v.type;
                    
                }
                else if (v.type != value_type)
                {
                    val = content.environment.GetType(v.type).ConvertTo(content.environment, v.value, value_type);
                   
                }

                content.DefineAndSet(value_name, value_type, val);
            }
            else
            {
                content.Define(value_name, value_type);
            }
            //设置环境变量为
            return null;
        }
        public string value_name;
        public Type value_type;
        public override string ToString()
        {
            string outs = "Define|" + value_type.Name + " " + value_name;
            if (_listParam != null)
            {
                if (_listParam.Count > 0)
                {
                    outs += "=";
                }
            }
            return outs;
        }
    }
}