using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{
    public partial class CLS_Expression_Compiler : ICLS_Expression_Compiler
    {

        public ICLS_Expression Compiler_Expression_Loop_For(IList<Token> tlist, CLS_Content content, int pos, int posend)
        {
            CLS_Expression_LoopFor value = new CLS_Expression_LoopFor();
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            
            int testbegin = fs1 + 1;
            if(b1!=1)
            {
                return null;
            }
            do
            {

                int fe2 = FindCodeAny(tlist, ref testbegin, out b1);


                ICLS_Expression subvalue;
                bool succ = Compiler_Expression(tlist,content, testbegin, fe2, out subvalue);
                if (!succ) return null;
                if (subvalue != null)
                {
                    value.listParam.Add(subvalue);
                    testbegin = fe2 + 2;
                }
                else
                {
                    value.listParam.Add(null);
                    testbegin = fe2 + 2;
                }
            }
            while (testbegin <= fe1);

            if(value.listParam.Count!=3)
            {
                return null;
            }
            ICLS_Expression subvalueblock;

            int b2;
            int fs2 = fe1 + 1;
            int fecode = FindCodeAny(tlist, ref fs2, out b2);
            bool succ2 = Compiler_Expression_Block(tlist,content, fs2, fecode, out subvalueblock);
            if(succ2)
            {
                value.listParam.Add(subvalueblock);
                return value;
            }
            return null;
        }

        public ICLS_Expression Compiler_Expression_Loop_If(IList<Token> tlist, CLS_Content content, int pos, int posend)
        {

            CLS_Expression_LoopIf value = new CLS_Expression_LoopIf();
            int b1;
            int fs1 = pos + 1;
            int fe1 = FindCodeAny(tlist, ref fs1, out b1);
            if (b1 != 1)
            {
                return null;
            }

            //if(xxx)
            {
                ICLS_Expression subvalue;
                bool succ = Compiler_Expression(tlist,content, fs1, fe1, out subvalue);
                if (succ)
                {
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            //if(...){yyy}

            int b2;
            int fs2 = fe1 + 1;
            int fe2 = FindCodeAny(tlist, ref fs2, out b2);
            {
                ICLS_Expression subvalue;
                bool succ = Compiler_Expression_Block(tlist,content, fs2, fe2, out subvalue);
                if (succ)
                {
                    value.listParam.Add(subvalue);
                }
                else
                {
                    return null;
                }
            }

            int nelse = fe2 + 1;
            if (b2 == 0) nelse++;
            int feelse = FindCodeAny(tlist, ref nelse, out b2);
            if (tlist.Count > nelse)
            {
                if (tlist[nelse].type == TokenType.KEYWORD && tlist[nelse].text == "else")
                { //if(...){...}else{zzz}
                    int b3;
                    int fs3 = nelse + 1;
                    int fe3 = FindCodeAny(tlist, ref fs3, out b3);
                    ICLS_Expression subvalue;
                    bool succ = Compiler_Expression_Block(tlist,content, fs3, fe3, out subvalue);
                    if (succ)
                    {
                        value.listParam.Add(subvalue);
                    }
                    else
                    {
                        return null;
                    }
                }
            }


            return value;
        }
        public ICLS_Expression Compiler_Expression_Loop_Return(IList<Token> tlist, CLS_Content content, int pos, int posend)
        {
            CLS_Expression_LoopReturn value = new CLS_Expression_LoopReturn();

            ICLS_Expression subvalue;
            bool succ = Compiler_Expression(tlist,content, pos + 1, posend, out subvalue);
            if (succ)
            {
                value.listParam.Add(subvalue);
            }

            return value;
        }
        public ICLS_Expression Compiler_Expression_Loop_Break(IList<Token> tlist, int pos)
        {
            CLS_Expression_LoopBreak value = new CLS_Expression_LoopBreak();
            return value;
        }
        public ICLS_Expression Compiler_Expression_Loop_Continue(IList<Token> tlist, int pos)
        {
            CLS_Expression_LoopContinue value = new CLS_Expression_LoopContinue();
            return value;
        }
    }
}