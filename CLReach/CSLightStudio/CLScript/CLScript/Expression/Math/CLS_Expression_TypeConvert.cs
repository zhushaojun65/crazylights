using System;
using System.Collections.Generic;
using System.Text;
namespace CLScript
{

    public class CLS_Expression_TypeConvert : ICLS_Expression
    {
        public CLS_Expression_TypeConvert()
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

            var right = listParam[0].ComputeValue(content);
            ICLS_Type type = content.environment.GetType(right.type);
            CLS_Content.Value value = new CLS_Content.Value();
            value.type = targettype;
            value.value = type.ConvertTo(content.environment, right.value, targettype);

            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            return value;
        }

        public Type type
        {
            get { return null; }
        }
        public Type targettype;

        public override string ToString()
        {
            return "convert<" + targettype.Name + ">";
        }
    }
}