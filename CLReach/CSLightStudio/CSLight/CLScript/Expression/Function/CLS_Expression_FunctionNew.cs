using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_FunctionNew: ICLS_Expression
    {
        public CLS_Expression_FunctionNew()
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
            List<CLS_Content.Value> list = new List<CLS_Content.Value>();
            foreach(ICLS_Expression p in listParam)
            {
                if(p!=null)
                {
                    list.Add(p.ComputeValue(content));
                }
            }
            return type.function.New(content.environment,list);

        }
        public CSLight.ICLS_Type type;
  
        public override string ToString()
        {
            return "new|" + type.keyword + "(params[" + listParam.Count + ")";
        }
    }
}