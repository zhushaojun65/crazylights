﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_LoopFor : ICLS_Expression
    {
        public CLS_Expression_LoopFor(int tbegin, int tend)
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
            set;
        }
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            content.DepthAdd();
            ICLS_Expression expr_init = listParam[0] as ICLS_Expression;
            if (expr_init != null) expr_init.ComputeValue(content);

            ICLS_Expression expr_continue = listParam[1] as ICLS_Expression;
            ICLS_Expression expr_step = listParam[2] as ICLS_Expression;

            ICLS_Expression expr_block = listParam[3] as ICLS_Expression;

            for (; (bool)expr_continue.ComputeValue(content).value; expr_step.ComputeValue(content))
            {
                if (expr_block != null)
                {
                    var v = expr_block.ComputeValue(content);
                    if (v != null && v.breakBlock > 1) break; ;
                    //if (v.breakBlock == 1) continue;
                    //if (v.breakBlock == 2) break;
                    //if (v.breakBlock == 10) return v;
                }
            }
            content.DepthRemove();
            content.OutStack(this);
            return null;
            //for 逻辑
            //做数学计算
            //从上下文取值
            //_value = null;
        }


        public override string ToString()
        {
            return "For|";
        }
    }
}