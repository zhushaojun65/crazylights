﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{

    public class CLS_Expression_LoopBreak : ICLS_Expression
    {
        public CLS_Expression_LoopBreak(int tbegin,int tend)
        {
            tokenBegin = tbegin;
            tokenEnd = tend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
        {
            get
            {
                return null;
            }
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
            CLS_Content.Value rv = new CLS_Content.Value();
            rv.breakBlock = 2;
            //跳出逻辑
            content.OutStack(this);
            return rv;
        }

        public override string ToString()
        {
            return "break;";
        }
    }
}