using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_LoopReturn : ICLS_Expression
    {
        public CLS_Expression_LoopReturn()
        {
            listParam = new List<ICLS_Expression>();
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get;
            private set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            CLS_Content.Value rv = new CLS_Content.Value();
            rv.breakBlock = 10;
            if (listParam.Count > 0&&listParam[0]!=null)
            {
                var v = listParam[0].ComputeValue(content);
                {
                    rv.type = v.type;
                    rv.value = v.value;
                }
            }
            else
            {
                rv.type = typeof(void);
            }
            return rv;

            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
        }


        public override string ToString()
        {
            return "return|";
        }
    }
}