﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CQuark
{
    class CQ_Type_Sbyte : RegHelper_Type
    {
        public CQ_Type_Sbyte()
            : base(typeof(sbyte), "sbyte",false)
        {

        }

        public override object ConvertTo(CQ_Content env, object src, CQType targetType)
        {
            bool convertSuccess = false;
            object convertedObject = NumericTypeUtils.TryConvertTo<sbyte>(src, targetType, out convertSuccess);
            if (convertSuccess) {
                return convertedObject;
            }

            return base.ConvertTo(env, src, targetType);
        }

        public override object Math2Value(CQ_Content env, char code, object left, CQ_Content.Value right, out CQType returntype)
        {
            bool math2ValueSuccess = false;
            object value = NumericTypeUtils.Math2Value<sbyte>(code, left, right, out returntype, out math2ValueSuccess);
            if (math2ValueSuccess) {
                return value;
            }

            return base.Math2Value(env, code, left, right, out returntype);
        }

        public override bool MathLogic(CQ_Content env, logictoken code, object left, CQ_Content.Value right)
        {
            bool mathLogicSuccess = false;
            bool value = NumericTypeUtils.MathLogic<sbyte>(code, left, right, out mathLogicSuccess);
            if (mathLogicSuccess) {
                return value;
            }

            return base.MathLogic(env, code, left, right);
        }

        public override object DefValue
        {
            get { return (sbyte)0; }
        }
    }
}
