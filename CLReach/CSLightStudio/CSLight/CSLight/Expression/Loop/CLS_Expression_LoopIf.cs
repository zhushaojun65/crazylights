using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_LoopIf : ICLS_Expression
    {
        public CLS_Expression_LoopIf()
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
            ICLS_Expression expr_if = listParam[0];
            bool bif = (bool)expr_if.ComputeValue(content).value;
            //if (expr_init != null) expr_init.ComputeValue(content);
            ICLS_Expression expr_go1 = listParam[1];
            ICLS_Expression expr_go2 = null;
            if(listParam.Count>2)expr_go2= listParam[2];
            if (bif && expr_go1 != null)
            {

                return expr_go1.ComputeValue(content);


            }
            else if (!bif && expr_go2 != null)
            {

                return expr_go2.ComputeValue(content);


            }

            //while((bool)expr_continue.value);

            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
            return null;
        }


        public override string ToString()
        {
            return "If|";
        }
    }
}