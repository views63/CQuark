﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CSLE
{

    public class CLS_Expression_Block : ICLS_Expression
    {
        public CLS_Expression_Block(int tbegin,int tend,int lbegin,int lend)
        {
            listParam = new List<ICLS_Expression>();
            tokenBegin = tbegin;
            tokenEnd = tend;
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
            content.DepthAdd();
            CLS_Content.Value value = null;
			foreach (ICLS_Expression i in listParam)
			{
				ICLS_Expression e =i  as ICLS_Expression;
				if (e != null)
					value =e.ComputeValue(content);

				if (value!=null&&value.breakBlock != 0) break;
			} 
            content.DepthRemove();
            content.OutStack(this);
			return value;
        }
		public IEnumerator CoroutineCompute(CLS_Content content, ICoroutine coroutine)
		{
			content.InStack(this);
			content.DepthAdd();
			CLS_Content.Value value = null;
			foreach (ICLS_Expression i in listParam)
			{
				ICLS_Expression e =i  as ICLS_Expression;
				if (e != null){
					if(e.hasCoroutine){
						yield return coroutine.StartNewCoroutine(e.CoroutineCompute(content, coroutine));
					}else{
						value =e.ComputeValue(content);
						if (value != null && value.breakBlock != 0)
							yield break;
					}
				}
			}
			content.DepthRemove();
			content.OutStack(this);
			yield break;
		}

  
        public override string ToString()
        {
            return "Block|";
        }
    }
}