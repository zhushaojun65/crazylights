﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSLight
{
    public class RegHelper_TypeFunction : ICLS_TypeFunction
    {
        Type type;
        public RegHelper_TypeFunction(Type type)
        {
            this.type = type;
        }
        public virtual CLS_Content.Value New(CLS_Environment environment, IList<CLS_Content.Value> _params)
        {

            List<Type> types = new List<Type>();
            List<object> objparams = new List<object>();
            foreach (var p in _params)
            {
                types.Add(p.type);
                objparams.Add(p.value);
            }
            CLS_Content.Value value = new CLS_Content.Value();
            value.type = type;
            var con = this.type.GetConstructor(types.ToArray());
            value.value = con.Invoke(objparams.ToArray());
            return value;
        }
        public virtual CLS_Content.Value StaticCall(CLS_Environment environment, string function, IList<CLS_Content.Value> _params)
        {

            List<object> _oparams = new List<object>();
            List<Type> types = new List<Type>();
            foreach (var p in _params)
            {
                _oparams.Add(p.value);
                types.Add(p.type);
            }
            var targetop = type.GetMethod(function, types.ToArray());
            CLS_Content.Value v = new CLS_Content.Value();
            v.value = targetop.Invoke(null, _oparams.ToArray());
            v.type = targetop.ReturnType;
            return v;


        }

        public virtual CLS_Content.Value StaticValueGet(CLS_Environment environment, string valuename)
        {
            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.value = targetp.GetValue(null, null);
                v.type = targetp.PropertyType;
                return v;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    CLS_Content.Value v = new CLS_Content.Value();
                    v.value = targetf.GetValue(null);
                    v.type = targetf.FieldType;
                    return v;
                }
            }
            return null;
        }

        public virtual void StaticValueSet(CLS_Environment environment, string valuename, object value)
        {

            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                if (value != null && value.GetType() != targetp.PropertyType)
                {
                    value = environment.GetType(value.GetType()).ConvertTo(environment, value, targetp.PropertyType);
                }
                targetp.SetValue(null, value, null);
                return;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    if (value != null && value.GetType() != targetf.FieldType)
                    {
                        value = environment.GetType(value.GetType()).ConvertTo(environment, value, targetf.FieldType);
                    }
                    targetf.SetValue(null, value);
                    return;
                }
            }



            throw new NotImplementedException();
        }

        public virtual CLS_Content.Value MemberCall(CLS_Environment environment, object object_this, string func, IList<CLS_Content.Value> _params)
        {

            List<Type> types = new List<Type>();
            List<object> _oparams = new List<object>();
            foreach (var p in _params)
            {
                _oparams.Add(p.value);
                types.Add(p.type);
            }

            var targetop = type.GetMethod(func, types.ToArray());
            CLS_Content.Value v = new CLS_Content.Value();
            if (targetop == null)
            {
                throw new Exception("函数不存在function:" + type.ToString() + "." + func );
            }
            v.value = targetop.Invoke(object_this, _oparams.ToArray());
            v.type = targetop.ReturnType;
            return v;
        }

        public virtual CLS_Content.Value MemberValueGet(CLS_Environment environment, object object_this, string valuename)
        {
            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                CLS_Content.Value v = new CLS_Content.Value();
                v.value = targetp.GetValue(object_this, null);
                v.type = targetp.PropertyType;
                return v;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    CLS_Content.Value v = new CLS_Content.Value();
                    v.value = targetf.GetValue(object_this);
                    v.type = targetf.FieldType;
                    return v;
                }
            }
            return null;
        }

        public virtual void MemberValueSet(CLS_Environment environment, object object_this, string valuename, object value)
        {

            var targetp = type.GetProperty(valuename);
            if (targetp != null)
            {
                if (value != null && value.GetType() != targetp.PropertyType)
                {
                    value = environment.GetType(value.GetType()).ConvertTo(environment, value, targetp.PropertyType);
                }

                targetp.SetValue(object_this, value, null);
                return;
            }
            else
            {
                var targetf = type.GetField(valuename);
                if (targetf != null)
                {
                    if (value != null && value.GetType() != targetf.FieldType)
                    {

                        value = environment.GetType(value.GetType()).ConvertTo(environment, value, targetf.FieldType);
                    }
                    targetf.SetValue(object_this, value);
                    return;
                }
            }



            throw new NotImplementedException();
        }




        public virtual CLS_Content.Value IndexGet(CLS_Environment environment, object object_this, object key)
        {
            //var m =type.GetMembers();
            var targetop = type.GetMethod("get_Item");
            if(targetop==null)
            {
                targetop = type.GetMethod("Get");
            }
            
            CLS_Content.Value v = new CLS_Content.Value();
            v.type = targetop.ReturnType;
            v.value = targetop.Invoke(object_this, new object[] { key });
            return v;

        }

        public virtual void IndexSet(CLS_Environment environment, object object_this, object key, object value)
        {
            var targetop = type.GetMethod("set_Item");
            targetop.Invoke(object_this, new object[] { key, value });
        }
    }

    public class RegHelper_Type : ICLS_Type
    {
        public RegHelper_Type(Type type, string setkeyword = null)
        {
            function = new RegHelper_TypeFunction(type);
            if (setkeyword != null)
            {
                keyword = setkeyword;
            }
            else
            {
                keyword = type.Name;
            }
            this.type = type;
        }

        public string keyword
        {
            get;
            protected set;
        }

        public Type type
        {
            get;
            protected set;
        }

        public virtual ICLS_Value MakeValue(object value) //这个方法可能存在AOT陷阱
        {
            //这个方法可能存在AOT陷阱
            //Type target = typeof(CLS_Value_Value<>).MakeGenericType(new Type[] { type }); 
            //return target.GetConstructor(new Type[] { }).Invoke(new object[0]) as ICLS_Value;

            CLS_Value_Object rvalue = new CLS_Value_Object(type);
            rvalue.value_value = value;
            return rvalue;
        }

        public virtual object ConvertTo(CLS_Environment env, object src, Type targetType)
        {

            //type.get
            //var m =type.GetMembers();
            var ms = type.GetMethods();
            foreach (var m in ms)
            {
                if ((m.Name == "op_Implicit" || m.Name == "op_Explicit") && m.ReturnType == targetType)
                {
                    return m.Invoke(null, new object[] { src });
                }
            }

            return src;
        }

        public virtual object Math2Value(CLS_Environment env, char code, object left, CLS_Content.Value right, out Type returntype)
        {
            returntype = type;
            System.Reflection.MethodInfo call = null;
            //var m = type.GetMethods();
            if (code == '+')
                call = type.GetMethod("op_Addition", new Type[] { this.type, right.type });
            else if (code == '-')//base = {CLScriptExt.Vector3 op_Subtraction(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_Subtraction",new Type[]{this.type,right.type});
            else if (code == '*')//[2] = {CLScriptExt.Vector3 op_Multiply(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_Multiply", new Type[] { this.type, right.type });
            else if (code == '/')//[3] = {CLScriptExt.Vector3 op_Division(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_Division", new Type[] { this.type, right.type });
            else if (code == '%')//[4] = {CLScriptExt.Vector3 op_Modulus(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_Modulus", new Type[] { this.type, right.type });
            var obj = call.Invoke(null, new object[] { left, right.value });
            //function.StaticCall(env,"op_Addtion",new List<ICL>{})
            return obj;
        }

        public virtual bool MathLogic(CLS_Environment env, logictoken code, object left, CLS_Content.Value right)
        {
            System.Reflection.MethodInfo call = null;

            //var m = type.GetMethods();
            if (code == logictoken.more)//[2] = {Boolean op_GreaterThan(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_GreaterThan");
            else if (code == logictoken.less)//[4] = {Boolean op_LessThan(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_LessThan");
            else if (code == logictoken.more_equal)//[3] = {Boolean op_GreaterThanOrEqual(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_GreaterThanOrEqual");
            else if (code == logictoken.less_equal)//[5] = {Boolean op_LessThanOrEqual(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_LessThanOrEqual");
            else if (code == logictoken.equal)//[6] = {Boolean op_Equality(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_Equality");
            else if (code == logictoken.not_equal)//[7] = {Boolean op_Inequality(CLScriptExt.Vector3, CLScriptExt.Vector3)}
                call = type.GetMethod("op_Inequality");
            var obj = call.Invoke(null, new object[] { left, right.value });
            return (bool)obj;
        }

        public ICLS_TypeFunction function
        {
            get;
            protected set;
        }
    }
}
