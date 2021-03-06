﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    public class RegHelper_Function : ICQ_Function
    {
        Delegate dele;
        public RegHelper_Function(Delegate dele)
        {
            this.dele = dele;

                this.keyword = dele.Method.Name;

            returntype = dele.Method.ReturnType;
            foreach (System.Reflection.ParameterInfo p in dele.Method.GetParameters())
            {
                defvalues.Add(p.DefaultValue);
                paramtype.Add(p.ParameterType);
            }
        }
        public RegHelper_Function(Delegate dele,string setkeyword)
        {
            this.dele = dele;
            if (setkeyword != null)
            {
                this.keyword = setkeyword;
            }
            else
            {
                this.keyword = dele.Method.Name;
            }
            returntype = dele.Method.ReturnType;
            foreach (System.Reflection.ParameterInfo p in dele.Method.GetParameters())
            {
                defvalues.Add(p.DefaultValue);
                paramtype.Add(p.ParameterType);
            }
        }
        List<object> defvalues = new List<object>();
        List<Type> paramtype = new List<Type>();
       
        public string keyword
        {
            get;
            private set;
        }
		public Type returntype {
			get;
			private set;
		}
        public CQ_Content.Value Call(CQ_Content content, IList<CQ_Content.Value> param)
        {
            CQ_Content.Value v = new CQ_Content.Value();
            List<object> objs = new List<object>();
            //var _params =   dele.Method.GetParameters();
            for (int i = 0; i < this.defvalues.Count; i++)
            {
                if (i >= param.Count)
                {
                    objs.Add(defvalues[i]);
                }
                else
                {
                    if (this.paramtype[i] == (Type)param[i].type)
                    {
                        objs.Add(param[i].value);
                    }
                    else
                    {
                        object conv = content.environment.GetType(param[i].type).ConvertTo(content, param[i].value, paramtype[i]);
                        objs.Add(conv);
                    }
                }
            }
            v.type = this.returntype;
            v.value = dele.DynamicInvoke(objs.ToArray());
            return v;
        }
    }

  
}
