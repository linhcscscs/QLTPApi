using AutoMapper;
using DataAccess.Dtos;
using DataAccess.SQL.QLTP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AutoMapper
{
    public partial class CommonMapper : Profile
    {
        public CommonMapper()
        {
            CreateMap<SO_GD, DmSoGdDto>();
        }
    }
}
