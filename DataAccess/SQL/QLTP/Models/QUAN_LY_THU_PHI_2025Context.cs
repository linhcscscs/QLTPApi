using DataAccess.Helper.ConfigHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SQL.QLTP.Models;

public partial class QUAN_LY_THU_PHI_2025Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConfigHelper.GetConnectionStringByName("QUAN_LY_THU_PHI_2025Context"));
}
