﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_Math2Value : ICLS_Expression
    {
        public CLS_Expression_Math2Value()
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
            CLS_Content.Value result = new CLS_Content.Value();


            {
                var left = listParam[0].ComputeValue(content);
                var right = listParam[1].ComputeValue(content);
                result.value = content.environment.GetType(left.type).Math2Value(content.environment, mathop, left.value, right, out result.type);
                return result;
            }

            return null;

        }

  
        public char mathop;
    

        public override string ToString()
        {
            return "Math2Value|a" + mathop + "b";
        }
    }
}