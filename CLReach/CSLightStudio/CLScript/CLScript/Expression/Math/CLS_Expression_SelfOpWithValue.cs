﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_SelfOpWithValue : ICLS_Expression
    {
        public CLS_Expression_SelfOpWithValue()
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
            var v = content.Get(value_name);
            var right = listParam[0].ComputeValue(content);
            ICLS_Type type = content.environment.GetType(v.type);
            //if (mathop == "+=")
            {
                Type returntype;
                object value = type.Math2Value(content.environment, mathop, v.value, right, out returntype);
                value = type.ConvertTo(content.environment, value, v.type);
                content.Set(value_name, value);
            }

            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            return null;
        }


        public string value_name;
        public char mathop;

        public override string ToString()
        {
            return "MathSelfOp|" + value_name + mathop;
        }
    }
}