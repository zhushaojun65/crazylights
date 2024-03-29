﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_NegativeValue : ICLS_Expression
    {
        public CLS_Expression_NegativeValue(int tbegin,int tend)
        {
            listParam = new List<ICLS_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get;
            private set;
        }
        public int tokenBegin
        {
            get;
            private set;
        }
        public int tokenEnd
        {
            get;
            private set;
        }
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);

            CLS_Content.Value r = listParam[0].ComputeValue(content);
            ICLS_Type type =content.environment.GetType(r.type);
            
            r.value= type.Math2Value(content.environment, '*', r.value, CLS_Content.Value.OneMinus, out r.type);
            content.OutStack(this);

            return r;
        }

   
        public override string ToString()
        {
            return "(-)|";
        }
    }
}