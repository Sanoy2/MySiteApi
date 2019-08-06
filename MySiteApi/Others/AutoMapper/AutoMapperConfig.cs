using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteApi.Others.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Configure() =>
            new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<type1, type2>();
            }).CreateMapper();
    }
}
