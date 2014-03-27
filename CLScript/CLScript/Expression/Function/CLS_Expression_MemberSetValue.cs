﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_MemberSetValue : ICLS_Expression
    {
        public CLS_Expression_MemberSetValue()
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
            var value = listParam[1].ComputeValue(content);
            object setv=value.value;
            //if(value.type!=parent.type)
            //{
            //    var vtype = content.environment.GetType(value.type);
            //    setv = vtype.ConvertTo(content.environment, setv, parent.type);
            //}
            var type = content.environment.GetType(parent.type);
            type.function.MemberValueSet(content.environment, parent.value, membername, setv);
            //做数学计算
            //从上下文取值
            //_value = null;
            return null;
        }

        public string membername;
     
        public override string ToString()
        {
            return "MemberSetvalue|a." + membername ;
        }
    }
}