﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_Block : ICLS_Expression
    {
        public CLS_Expression_Block()
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
            CLS_Content.Value value = null;
            foreach (ICLS_Expression i in listParam)
            {
                ICLS_Expression e =i  as ICLS_Expression;
                if (e != null)
                    value =e.ComputeValue(content);


                if (value!=null&&value.breakBlock != 0) break;
            }
            return value;
        }

  
        public override string ToString()
        {
            return "Block|";
        }
    }
}