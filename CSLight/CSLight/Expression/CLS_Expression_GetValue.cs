using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_GetValue : ICLS_Expression
    {
        public CLS_Expression_GetValue()
        {
          
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get
            {
                return null;
            }
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            var value=content.Get(value_name);
            content.OutStack(this);

            //从上下文取值

            return value;
        }

        public string value_name;
    
        public override string ToString()
        {
            return "GetValue|" + value_name;
        }
    }
}