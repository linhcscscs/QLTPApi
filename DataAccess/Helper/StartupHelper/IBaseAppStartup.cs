using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper.StartupHelper
{
    public interface IBaseAppStartup
    {
        void Configure(WebApplication app);
    }
}
