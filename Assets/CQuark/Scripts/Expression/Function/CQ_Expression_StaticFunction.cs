﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_StaticFunction : ICQ_Expression
    {
        public CQ_Expression_StaticFunction(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICQ_Expression>();
            this.tokenBegin = tbegin;
            this.tokenEnd = tend;
            lineBegin = lbegin;
            lineEnd = lend;
        }
        public int lineBegin
        {
            get;
            private set;
        }
        public int lineEnd
        {
            get;
            private set;
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
            private set;
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
        MethodCache cache = null;
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            //var parent = listParam[0].ComputeValue(content);
            //var type = content.environment.GetType(parent.type);
            List<CQ_Content.Value> _params = new List<CQ_Content.Value>();
            for (int i = 0; i < listParam.Count; i++)
            {
                _params.Add(listParam[i].ComputeValue(content));
            }
            CQ_Content.Value value = null;
            if (cache == null || cache.cachefail)
            {
                cache = new MethodCache();
                value = type.function.StaticCall(content, functionName, _params, cache);
            }
            else
            {
                value = type.function.StaticCallCache(content, _params, cache);
            }
            
            content.OutStack(this);
            return value;
            //做数学计算
            //从上下文取值
            //_value = null;
            //return null;
        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public ICQ_Type type;
        public string functionName;

        public override string ToString()
        {
            return "StaticCall|" + type.keyword + "." + functionName;
        }
    }
}