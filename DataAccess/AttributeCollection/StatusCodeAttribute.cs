using DataAccess.Helper.Extensions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AttributeCollection
{
    class StatusCodeAttribute : Attribute, ICustomAttribute<int>
    {
        private int _value;
        public StatusCodeAttribute(int value) { _value = value; }

        public int GetValue()
        {
            return _value;
        }
    }
}
