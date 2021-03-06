﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

	public class CQ_Expression_Function : ICQ_Expression
    {
        public CQ_Expression_Function(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICQ_Expression> listParam
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
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            set;
        }
		public bool hasCoroutine{
			get{
				if(listParam == null || listParam.Count == 0)
					return false;
				foreach(ICQ_Expression expr in listParam){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            List<CQ_Content.Value> list = new List<CQ_Content.Value>();
            foreach (ICQ_Expression p in listParam)
            {
                if (p != null)
                {
                    list.Add(p.ComputeValue(content));
                }
            }
			CQ_Content.Value v = null;

            SType.Function retFunc = null;
            bool bFind = false;
            if (content.CallType != null)
                bFind = content.CallType.functions.TryGetValue(funcname, out retFunc);

            if (bFind)
            {
                if (retFunc.bStatic)
                {
                    v = content.CallType.StaticCall(content, funcname, list);

                }
                else
                {
                    v = content.CallType.MemberCall(content, content.CallThis, funcname, list);

                }
            }


            else
            {
                v = content.GetQuiet(funcname);
                if (v != null && v.value is Delegate)
                {
                    //if(v.value is Delegate)
                    {
                        Delegate d = v.value as Delegate;
                        v = new CQ_Content.Value();
                        object[] obja = new object[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            obja[i] = list[i].value;
                        }
                        v.value = d.DynamicInvoke(obja);
                        if (v.value == null)
                        {
                            v.type = null;
                        }
                        else
                        {
                            v.type = v.value.GetType();
                        }
                    }
                    //else
                    //{
                    //    throw new Exception(funcname + "不是函数");
                    //}
                }
                else
                {
                    v = content.environment.GetFunction(funcname).Call(content, list);
                }
            }
            //操作变量之
            //做数学计算
            //从上下文取值
            //_value = null;
            content.OutStack(this);
            return v;
        }

		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}
        public string funcname;

        public override string ToString()
        {

            return "Call|" + funcname + "(params[" + listParam.Count + ")";
        }
    }
}