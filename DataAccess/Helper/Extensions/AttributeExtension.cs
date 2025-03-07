using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Helper.Extensions.Interface;

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
        public static valueType? GetAttribute<Atribute, valueType>(this Enum GenericEnum)
            where Atribute : Attribute, ICustomAttribute<valueType>
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
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
