using System;
using System.Collections.Generic;
using System.Text;

namespace CSLight
{
    public enum TokenType
    {
        UNKNOWN,
        KEYWORD,        //关键字
        PUNCTUATION,    //标点
        IDENTIFIER,     //标识符 变量与函数
        TYPE,           //类型
        COMMENT,        //注释
        VALUE,          //数值
        STRING,         //字符串
    }
    public struct Token
    {
        //asTC_UNKNOWN    = 0,//未知
        //asTC_KEYWORD    = 1,//关键字
        //asTC_VALUE      = 2,//value   值
        //asTC_IDENTIFIER = 3,//标识符  变量
        //asTC_COMMENT    = 4,//注释
        //asTC_WHITESPACE = 5,//空格

        public string text;
        public int col;
        public TokenType type;
        public override string ToString()
        {
            return type.ToString() + "|" + text + "|" + col.ToString();
        }
    }
    public interface ICLS_TokenParser
    {
        List<string> types
        {
            get;
        }
        List<string> keywords
        {
            get;
        }
        IList<Token> Parse(string line);
    }
    public class CLS_TokenParser : ICLS_TokenParser
    {
        public CLS_TokenParser()
        {
            types = new List<string>();
            keywords = new List<string>();
            types.Add("void");
            types.Add("bool");
            types.Add("int");
            types.Add("uint");
            types.Add("float");
            types.Add("double");
            types.Add("string");

            keywords.Add("if");
            keywords.Add("as");
            keywords.Add("else");
            keywords.Add("break");
            keywords.Add("continue");
            keywords.Add("for");
            keywords.Add("trace");
            keywords.Add("return");
            keywords.Add("true");
            keywords.Add("false");
            keywords.Add("null");
            keywords.Add("new");
        }
        public List<string> types
        {
            get;
            private set;
        }
        public List<string> keywords
        {
            get;
            private set;
        }

        int FindStart(string line, int npos)
        {
            int n = npos;
            for (int i = n; i < line.Length; i++)
            {
                if (!char.IsSeparator(line, i) && line[i] != '\n' &&line[i]!='\r')
                {
                    return i;
                }
            }
            return -1;
        }
        int GetToken(string line, int npos, out Token t)
        {
            //找到开始字符
            int nstart = FindStart(line, npos);
            t.col = nstart;
            t.text = " ";
            t.type = TokenType.UNKNOWN;
            if (nstart < 0) return -1;
            if (line[nstart] == '\"')
            {
                //字符串查找
                int nend = line.IndexOf('\"', nstart + 1);
                int nsub = line.IndexOf('\\', nstart + 1);
                while (nsub > 0 && nsub < nend)
                {
                    nend = line.IndexOf('\"', nsub + 2);
                    nsub = line.IndexOf('\\', nsub + 2);

                }

                t.type = TokenType.STRING;
                int pos = nend + 1;
                t.text = line.Substring(nstart, nend - nstart + 1);
                t.text = t.text.Replace("\\\"", "\"");
                t.text = t.text.Replace("\\\'", "\'");
                t.text = t.text.Replace("\\\\", "\\");
                t.text = t.text.Replace("\\0", "\0");
                t.text = t.text.Replace("\\a", "\a");
                t.text = t.text.Replace("\\b", "\b");
                t.text = t.text.Replace("\\f", "\f");
                t.text = t.text.Replace("\\n", "\n");
                t.text = t.text.Replace("\\r", "\r");
                t.text = t.text.Replace("\\t", "\t");
                t.text = t.text.Replace("\\v", "\v");
                int sp = t.text.IndexOf('\\');
                if (sp > 0)
                {
                    throw new Exception("不可识别的转义序列:" + t.text.Substring(sp));
                }
                return pos;
            }
            else if (line[nstart] == '/')// / /= 注释
            {

                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                {
                    t.type = TokenType.PUNCTUATION;
                    t.text = line.Substring(nstart, 2);
                }
                else if (nstart < line.Length - 1 && line[nstart + 1] == '/')
                {
                    t.type = TokenType.COMMENT;
                    int enterpos = line.IndexOf('\n', nstart + 2);
                    if (enterpos < 0) t.text = line.Substring(nstart);
                    else
                        t.text = line.Substring(nstart, line.IndexOf('\n', nstart + 2) - nstart);
                }
                else
                {
                    t.type = TokenType.PUNCTUATION;
                    t.text = line.Substring(nstart, 1);
                }
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '=')//= ==
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '!')//= ==
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '+')//+ += ++
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && (line[nstart + 1] == '=' || line[nstart + 1] == '+'))
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            //通用的一元二元运算符检查
            else if (line[nstart] == '-')//- -= -- 负数也先作为符号处理
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=' || line[nstart + 1] == '-')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '*')//* *=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '/')/// /=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '%')/// /=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '>')//> >=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }
            else if (line[nstart] == '<')//< <=
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '=')
                    t.text = line.Substring(nstart, 2);
                else
                    t.text = line.Substring(nstart, 1);
                return nstart + t.text.Length;
            }

            else if (line[nstart] == '&')//&&
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '&')
                    t.text = line.Substring(nstart, 2);
                else
                    return -1;
            }
            else if (line[nstart] == '|')//||
            {
                t.type = TokenType.PUNCTUATION;
                if (nstart < line.Length - 1 && line[nstart + 1] == '|')
                    t.text = line.Substring(nstart, 2);
                else
                    return -1;
            }
            else if (char.IsLetter(line, nstart) || line[nstart] == '_')
            {
                //字母逻辑
                //判断完整性
                int i = nstart + 1;
                while (i < line.Length - 1 && (char.IsLetterOrDigit(line, i) || line[i] == '_'))
                {
                    i++;
                }
                t.text = line.Substring(nstart, i - nstart);
                //判断字母类型： 关键字 类型 标识符
                foreach (string s in keywords)
                {
                    if (t.text == s)
                    {
                        t.type = TokenType.KEYWORD;
                        return nstart + t.text.Length;
                    }
                }
                foreach (string s in types)
                {
                    if (t.text == s)
                    {
                        t.type = TokenType.TYPE;
                        return nstart + t.text.Length;
                    }
                }
                if(line[i]=='<'|| line[i]=='[')//检查特别类型
                {
                    foreach (string s in types)
                    {
                        if (line.IndexOf(s, nstart) == nstart)
                        {
                            t.type = TokenType.TYPE;
                            t.text = s;
                            return nstart + s.Length;
                        }

                    }
                }
                t.type = TokenType.IDENTIFIER;
                return nstart + t.text.Length;
            }
            else if (char.IsPunctuation(line, nstart))
            {



                //else
                {
                    t.type = TokenType.PUNCTUATION;
                    t.text = line.Substring(nstart, 1);
                    return nstart + t.text.Length;
                }
                //符号逻辑
                //-号逻辑
                //"号逻辑
                ///逻辑
                //其他符号
            }
            else if (char.IsNumber(line, nstart))
            {
                //数字逻辑
                //判断数字合法性

                if (line[nstart] == '0' && line[nstart + 1] == 'x') //0x....
                {
                    int iend = nstart + 2;
                    for (int i = nstart + 2; i < line.Length; i++)
                    {
                        if (char.IsNumber(line, i))
                        {
                            iend = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    t.type = TokenType.VALUE;
                    t.text = line.Substring(nstart, iend - nstart + 1);
                }
                else
                {
                    //纯数字

                    int iend = nstart;
                    for (int i = nstart + 1; i < line.Length; i++)
                    {
                        if (char.IsNumber(line, i))
                        {
                            iend = i;
                        }
                        else
                        {
                            break;
                        }
                    }
                    t.type = TokenType.VALUE;
                    int dend = iend + 1;
                    if (dend < line.Length && line[dend] == '.')
                    {
                        int fend = dend;
                        for (int i = dend + 1; i < line.Length; i++)
                        {
                            if (char.IsNumber(line, i))
                            {
                                fend = i;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (fend + 1 < line.Length && line[fend + 1] == 'f')
                        {
                            t.text = line.Substring(nstart, fend + 2 - nstart);

                        }
                        else
                        {
                            t.text = line.Substring(nstart, fend + 1 - nstart);
                        }
                        //.111
                        //.123f
                    }
                    else
                    {
                        if (dend  < line.Length && line[dend ] == 'f')
                        {
                            t.text = line.Substring(nstart, dend - nstart+1);
                        }
                        else
                        {
                            t.text = line.Substring(nstart, dend - nstart);
                        }
                    }

                }
                return nstart + t.text.Length;
            }
            else
            {
                //不可识别逻辑
                int i = nstart + 1;
                while (i < line.Length - 1 && char.IsSeparator(line, i) == false && line[i] != '\n' && line[i] != '\r')
                {
                    i++;
                }
                t.text = line.Substring(nstart, i - nstart);
                return nstart + t.text.Length;
            }
            //
            //    -逻辑
            //
            //    "逻辑
            //
            //    /逻辑
            //
            //    其他符号逻辑


            return nstart + t.text.Length;
        }
        public IList<Token> Parse(string line)
        {
            List<Token> ts = new List<Token>();
            int n = 0;
            while (n >= 0)
            {
                Token t;
                n = GetToken(line, n, out t);
                if (n >= 0)
                {
                    if (t.type== TokenType.IDENTIFIER&& ts[ts.Count - 1].text == "." && ts[ts.Count - 2].type == TokenType.TYPE)
                    {
                        string ntype = ts[ts.Count - 2].text + ts[ts.Count - 1].text + t.text;
                        if(types.Contains(ntype))
                        {//类中类，合并之
                            t.type = TokenType.TYPE;
                            t.text = ntype;
                            t.col = ts[ts.Count - 2].col;
                            ts.RemoveAt(ts.Count - 1);
                            ts.RemoveAt(ts.Count - 1);

                        }
                    }

                    ts.Add(t);

                }
            }
            return ts;
        }
    }
}
