using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_Function: ICLS_Expression
    {
        public CLS_Expression_Function()
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
            var v=content.environment.GetFunction(funcname).Call(content.environment, list);
             //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            return v;
        }
        public string funcname;
  
        public override string ToString()
        {
           
                return "Call|" + funcname + "(params["+listParam.Count+")";
        }
    }
}