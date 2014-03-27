using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_LoopContinue : ICLS_Expression
    {
        public CLS_Expression_LoopContinue()
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
            CLS_Content.Value rv = new CLS_Content.Value();
            rv.breakBlock = 1;
            //跳出逻辑
            return rv;
        }

        public override string ToString()
        {
            return "continue;";
        }
    }
}