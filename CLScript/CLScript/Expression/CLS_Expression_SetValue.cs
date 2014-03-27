﻿using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_SetValue : ICLS_Expression
    {
        public CLS_Expression_SetValue()
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
           
            {
                CLS_Content.Value v = listParam[0].ComputeValue(content);
                Type value_type = content.values[value_name].type;

                object val = v.value;
                if (value_type != typeof(CLS_Type_Var.var) && value_type!=v.type)
                {
                    val = content.environment.GetType(v.type).ConvertTo(content.environment, v.value, value_type);
                }
                content.Set(value_name, val);
            }
            return null;
        }


        public string value_name;
   
        public override string ToString()
        {
            return "SetValue|" + value_name +"=";
        }
    }
}