using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.Extensions.Interface
{
    public interface ICustomAttribute<T>
    {
        public T GetValue();
    }
}
