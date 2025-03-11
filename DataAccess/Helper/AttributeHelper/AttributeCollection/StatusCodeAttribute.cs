using DataAccess.Helper.Extensions.Interface;

namespace DataAccess.Helper.AttributeHelper.AttributeCollection
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
