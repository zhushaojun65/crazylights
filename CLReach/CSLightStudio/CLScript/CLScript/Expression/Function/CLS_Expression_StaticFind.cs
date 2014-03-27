using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_StaticFind : ICLS_Expression
    {
        public CLS_Expression_StaticFind()
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


            return type.function.StaticValueGet(content.environment, staticmembername);


        }

   
        public ICLS_Type type;
        public string staticmembername;
  
        public override string ToString()
        {
            return "StaticFind|" + type.keyword + "." + staticmembername;
        }
    }
}