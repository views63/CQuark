﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    /// <summary>
    /// 支持有返回值的 不带参数 的委托注册.
    /// 注意这里和void类型委托注册的用法有些区别：
    /// 这里的类模板第一个参数是返回类型.
    ///    比如有个返回bool型的委托定义如下：
    ///    public class Class {
    ///         public delegate bool BoolDelegate();
    ///    }
    ///    那么注册方式如下：
    ///    env.RegType(new RegHelper_DeleNonVoidAction<bool>(typeof(Class.BoolDelegate), "Class.BoolDelegate"));
    /// </summary>
    public class RegHelper_DeleNonVoidAction<ReturnType> : RegHelper_Type, ICQ_Type_Dele
    {
        /// <summary>
        /// 有返回值,同时不带参数的委托.
        /// </summary>
        /// <returns></returns>
        public delegate ReturnType NonVoidDelegate();

        public RegHelper_DeleNonVoidAction(Type type, string setkeyword)
            : base(type, setkeyword, true)
        {

        }

        public override object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            returntype = null;

            if (left is DeleEvent)
            {
                DeleEvent info = left as DeleEvent;
                Delegate calldele = null;

                //!--exist bug.
                /*if (right.value is DeleFunction) calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                else if (right.value is DeleLambda) calldele = CreateDelegate(env.environment, right.value as DeleLambda);
                else if (right.value is Delegate) calldele = right.value as Delegate;*/

                object rightValue = right.value;
                if (rightValue is DeleFunction)
                {
                    if (code == '+')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleFunction);
                    }
                    else if (code == '-')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleFunction);
                    }
                }
                else if (rightValue is DeleLambda)
                {
                    if (code == '+')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleLambda);
                    }
                    else if (code == '-')
                    {
                        calldele = CreateDelegate(env.environment, rightValue as DeleLambda);
                    }
                }
                else if (rightValue is Delegate)
                {
                    calldele = rightValue as Delegate;
                }

                if (code == '+')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    //if (!(rightValue is Delegate)) {
                    //    Dele_Map_Delegate.Map(rightValue as IDeleBase, calldele);
                    //}
                    return info;
                }
                else if (code == '-')
                {
                    info._event.RemoveEventHandler(info.source, calldele);
                    //if (!(rightValue is Delegate)) {
                    //    Dele_Map_Delegate.Destroy(rightValue as IDeleBase);
                    //}
                    return info;
                }

            }
            else if (left is Delegate || left == null)
            {
                Delegate info = left as Delegate;
                Delegate calldele = null;
                if (right.value is DeleFunction)
                    calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                else if (right.value is DeleLambda)
                    calldele = CreateDelegate(env.environment, right.value as DeleLambda);
                else if (right.value is Delegate)
                    calldele = right.value as Delegate;
                if (code == '+')
                {
                    return Delegate.Combine(info, calldele); ;
                }
                else if (code == '-')
                {
                    return Delegate.Remove(info, calldele);
                }
            }
            return new NotSupportedException();
        }
        public Delegate CreateDelegate(ICQ_Environment env, DeleFunction delefunc)
        {
            DeleFunction _func = delefunc;
            Delegate _dele = delefunc.cacheFunction(this._type, null);
            if (_dele != null) return _dele;
            NonVoidDelegate dele = delegate()
            {
                var func = _func.calltype.functions[_func.function];
                if (func.expr_runtime != null)
                {
                    CQ_Content content = new CQ_Content(env, true);
                    try
                    {
                        content.DepthAdd();
                        content.CallThis = _func.callthis;
                        content.CallType = _func.calltype;
                        content.function = _func.function;

                        CQ_Content.Value retValue = func.expr_runtime.ComputeValue(content);
                        content.DepthRemove();
                        return (ReturnType)retValue.value;
                    }
                    catch (Exception err)
                    {
                        string errinfo = "Dump Call in:";
                        if (_func.calltype != null) errinfo += _func.calltype.Name + "::";
                        if (_func.function != null) errinfo += _func.function;
                        errinfo += "\n";
                        env.logger.Log(errinfo + content.Dump()); 
                        throw err;
                    }
                }
                return default(ReturnType);
            };
            _dele = Delegate.CreateDelegate(this.type, dele.Target, dele.Method);
            return delefunc.cacheFunction(this._type, _dele);
        }

        public Delegate CreateDelegate(ICQ_Environment env, DeleLambda lambda)
        {
            CQ_Content content = lambda.content.Clone();
            //var pnames = lambda.paramNames;
            var expr = lambda.expr_func;

            NonVoidDelegate dele = delegate()
            {
                if (expr != null)
                {
                    try
                    {
                        content.DepthAdd();
                        CQ_Content.Value retValue = expr.ComputeValue(content);
                        content.DepthRemove();
                        return (ReturnType)retValue.value;
                    }
                    catch (Exception err)
                    {
                        string errinfo = "Dump Call lambda in:";
                        if (content.CallType != null) errinfo += content.CallType.Name + "::";
                        if (content.function != null) errinfo += content.function;
                        errinfo += "\n";
                        env.logger.Log(errinfo + content.Dump());
                        throw err;
                    }
                }
                return default(ReturnType);
            };

            Delegate d = dele;
            return Delegate.CreateDelegate(this.type, d.Target, d.Method);
        }
    }
}
