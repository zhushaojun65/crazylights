﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CSLight
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {
        ICLS_Logger logger;
        public CLS_Expression_Compiler(ICLS_Logger logger)
        {
            this.logger = logger;
        }
        public ICLS_Expression Compiler(IList<Token> tlist, CLS_Content content)
        {
            ICLS_Expression value;

            int expbegin = 0;
            int expend = FindCodeBlock(tlist, expbegin);
            if (expend != tlist.Count - 1)
            {
                LogError(tlist,"CodeBlock 识别问题,异常结尾",expbegin,expend);
                return null;
            }
            bool succ = Compiler_Expression_Block(tlist,content, expbegin, expend, out value);
            if (succ)
            {
                if (value == null)
                {
                    logger.Log_Warn("编译为null:");
                }
                return value;

            }
            else
            {
                LogError(tlist,"编译失败:" , expbegin ,expend);
                return null;
            }



        }

        public ICLS_Expression Compiler_NoBlock(IList<Token> tlist, CLS_Content content)
        {
            ICLS_Expression value;
            int expbegin = 0;
            int expend = tlist.Count - 1;
            bool succ = Compiler_Expression(tlist,content, expbegin, expend, out value);
            if (succ)
            {
                if (value == null)
                {
                    logger.Log_Warn("编译为null:");
                }
                return value;

            }
            else
            {
                LogError(tlist,"编译失败:", expbegin ,expend);
                return null;
            }


        }
        public ICLS_Expression Optimize(ICLS_Expression value,CLS_Content content)
        {
            ICLS_Expression expr = value as ICLS_Expression;
            if (expr == null) return value;
            else return OptimizeDepth(expr, content);
        }
        ICLS_Expression OptimizeDepth(ICLS_Expression expr, CLS_Content content)
        {
            //先进行深入优化
            if (expr.listParam != null)
            {
                for (int i = 0; i < expr.listParam.Count; i++)
                {
                    ICLS_Expression subexpr = expr.listParam[i] as ICLS_Expression;
                    if (subexpr != null)
                    {
                        expr.listParam[i] = OptimizeDepth(subexpr, content);
                    }
                }
            }


            return OptimizeSingle(expr, content);

        }
        ICLS_Expression OptimizeSingle(ICLS_Expression expr, CLS_Content content)
        {

            if (expr is CLS_Expression_Math2Value)
            {

                if (expr.listParam[0] is ICLS_Value &&
                expr.listParam[1] is ICLS_Value)
                {
                    CLS_Content.Value result = expr.ComputeValue(content);
                    if (result.type == typeof(bool))
                    {
                        CLS_Value_Value<bool> value = new CLS_Value_Value<bool>();
                        value.value_value = (bool)result.value;
                        return value;
                    }
                    else
                    {
                        ICLS_Type v = content.environment.GetType(result.type);
                        ICLS_Value value = v.MakeValue(result.value);
                        return value;
                    }

              
                }
            }
            return expr;
        }
    }
}