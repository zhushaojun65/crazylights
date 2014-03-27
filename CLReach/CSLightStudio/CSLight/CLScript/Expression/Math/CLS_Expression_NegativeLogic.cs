using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_NegativeLogic : ICLS_Expression
    {
        public CLS_Expression_NegativeLogic()
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
            CLS_Content.Value r = listParam[0].ComputeValue(content);
            r.value = !(bool)r.value;
            return r;
        }


        public override string ToString()
        {
            return "(!)|" ;
        }
    }
}