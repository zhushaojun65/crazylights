using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_MemberFind : ICLS_Expression
    {
        public CLS_Expression_MemberFind()
        {
           listParam= new List<ICLS_Expression>();
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get;
            private set;
        }

        public CLS_Content.Value ComputeValue(CLS_Content content)
        {

            var parent = listParam[0].ComputeValue(content);
            var type = content.environment.GetType(parent.type);

            return type.function.MemberValueGet(content.environment, parent.value, membername);
            //做数学计算
            //从上下文取值
            //_value = null;
            //return null;
        }

   
        public string membername;
   
        public override string ToString()
        {
            return "MemberFind|a." + membername;
        }
    }
}