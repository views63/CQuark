﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CSLE
{

    public class CLS_Expression_IndexSetValue : ICLS_Expression
    {
        public CLS_Expression_IndexSetValue(int tbegin, int tend, int lbegin, int lend)
        {
           listParam= new List<ICLS_Expression>();
           this.tokenBegin = tbegin;
           this.tokenEnd = tend;
           lineBegin = lbegin;
           lineEnd = lend;
        }
        //Block的参数 一个就是一行，顺序执行，没有
        public List<ICLS_Expression> listParam
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
		public bool hasCoroutine{
			get{
				if(listParam == null || listParam.Count == 0)
					return false;
				foreach(ICLS_Expression expr in listParam){
					if(expr.hasCoroutine)
						return true;
				}
				return false;
			}
		}
        public CLS_Content.Value ComputeValue(CLS_Content content)
        {
            content.InStack(this);
            var parent = listParam[0].ComputeValue(content);
            if (parent == null)
            {
                throw new Exception("调用空对象的方法:" + listParam[0].ToString() + ":" + ToString());
            }
            var key = listParam[1].ComputeValue(content);
            var value = listParam[2].ComputeValue(content);
            //object setv=value.value;
            //if(value.type!=parent.type)
            //{
            //    var vtype = content.environment.GetType(value.type);
            //    setv = vtype.ConvertTo(content.environment, setv, parent.type);
            //}
            var type = content.environment.GetType(parent.type);
            type.function.IndexSet(content, parent.value, key.value, value.value);
            //做数学计算
            //从上下文取值
            //_value = null;
            content.OutStack(this);
            return null;
        }
		public IEnumerator CoroutineCompute(CLS_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}


        public override string ToString()
        {
            return "IndexSet[]=|";
        }
    }
}