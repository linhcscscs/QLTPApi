using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccess.AttributeCollection;
using DataAccess.Helper.Extensions.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataAccess.Helper.Extensions
{
    public static class AttributeExtension
    {
        #region Attribute Extension
        public static string GetDescription(this object obj)
        {
            Type genericEnumType = obj.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(obj.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (_Attribs != null && _Attribs.Count() > 0)
                {
                    return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return obj.ToString();
        }
        public static int GetStatusCode(this object obj)
        {
            var ret = GetAttribute<StatusCodeAttribute, int>(obj);
            if (ret == 0) ret = 500;
            return ret;
        }
        public static valueType? GetAttribute<Atribute, valueType>(this object obj)
            where Atribute : Attribute, ICustomAttribute<valueType>
        {
            Type genericEnumType = obj.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(obj.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(Atribute), false);
                if (_Attribs != null && _Attribs.Count() > 0)
                {
                    return ((Atribute)_Attribs.ElementAt(0)).GetValue();
                }
            }
            return default;
        }
        #endregion
    }
}
