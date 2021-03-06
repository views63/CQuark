﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CQuark
{

    public class CQ_Expression_MemberMath : ICQ_Expression
    {
        public CQ_Expression_MemberMath(int tbegin, int tend, int lbegin, int lend)
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
        public CQ_Content.Value ComputeValue(CQ_Content content)
        {
            content.InStack(this);
            var parent = listParam[0].ComputeValue(content);
            if (parent == null)
            {
                throw new Exception("调用空对象的方法:" + listParam[0].ToString() + ":" + ToString());
            }
            var type = content.environment.GetType(parent.type);
            //string membername=null;


            var getvalue = type.function.MemberValueGet(content, parent.value, membername);

            CQ_Content.Value vright = CQ_Content.Value.One;
            if (listParam.Count > 1)
            {
                vright = listParam[1].ComputeValue(content);
            }
            CQ_Content.Value vout =new CQ_Content.Value();
            var mtype = content.environment.GetType(getvalue.type);
            vout.value = mtype.Math2Value(content, mathop, getvalue.value, vright, out vout.type);

            type.function.MemberValueSet(content, parent.value, membername, vout.value);
            //CQ_Content.Value v = new CQ_Content.Value();

            content.OutStack(this);
            return vout;
            //做数学计算
            //从上下文取值
            //_value = null;
            //return null;

        }
		public IEnumerator CoroutineCompute(CQ_Content content, ICoroutine coroutine)
		{
			throw new Exception ("暂时不支持套用协程");
		}

        public string membername;
        public char mathop;
        public override string ToString()
        {
            return "CQ_Expression_MemberMath|a." + membername + " |" + mathop;
        }
    }
}