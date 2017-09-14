﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CSLE
{

    public class CLS_Expression_SetValue : ICLS_Expression
    {
        public CLS_Expression_SetValue(int tbegin, int tend, int lbegin, int lend)
        {
            listParam = new List<ICLS_Expression>();
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

            {

                CLS_Content.Value v = listParam[0].ComputeValue(content);

                {
                    object val = v.value;
                    CLType value_type = null;
                    if (content.values.ContainsKey(value_name))
                    {
                        value_type = content.values[value_name].type;
                    }
                    else
                    {
                        if (content.CallType != null)
                        {
                            if (content.CallType.members.ContainsKey(value_name))
                            {
                                if (content.CallType.members[value_name].bStatic)
                                {
                                    value_type = content.CallType.staticMemberInstance[value_name].type;
                                }
                                else
                                {
                                    value_type = content.CallThis.member[value_name].type;
                                }

                            }

                        }
                    }
                    //val = v.value;
                    if ((Type)value_type != typeof(CLS_Type_Var.var) && value_type != v.type)
                    {
                        val = content.environment.GetType(v.type).ConvertTo(content, v.value, value_type);
                    }


                    content.Set(value_name, val);
                }
            }
            content.OutStack(this);
            return null;
        }
		public IEnumerator CoroutineCompute(CLS_Content content, ICoroutine coroutine)
		{
			content.InStack(this);
			{
				if(listParam[0].hasCoroutine){
					yield return coroutine.StartNewCoroutine(listParam[0].CoroutineCompute(content, coroutine));
				}else{
					CLS_Content.Value v = listParam[0].ComputeValue(content);
					
					{
						object val = v.value;
						CLType value_type = null;
						if (content.values.ContainsKey(value_name))
						{
							value_type = content.values[value_name].type;
						}
						else
						{
							if (content.CallType != null)
							{
								if (content.CallType.members.ContainsKey(value_name))
								{
									if (content.CallType.members[value_name].bStatic)
									{
										value_type = content.CallType.staticMemberInstance[value_name].type;
									}
									else
									{
										value_type = content.CallThis.member[value_name].type;
									}
									
								}
								
							}
						}
						//val = v.value;
						if ((Type)value_type != typeof(CLS_Type_Var.var) && value_type != v.type)
						{
							val = content.environment.GetType(v.type).ConvertTo(content, v.value, value_type);
						}
						
						
						content.Set(value_name, val);
					}
				}
			}
			content.OutStack(this);
		}

        public string value_name;

        public override string ToString()
        {
            return "SetValue|" + value_name + "=";
        }
    }
}