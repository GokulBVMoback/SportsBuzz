using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AutoMapper<Tsource, TDestination>
    {
        private static Mapper _mapper = new Mapper(new MapperConfiguration(
           cfg => cfg.CreateMap<Tsource, TDestination>()));

        public static List<TDestination> MapList(List<Tsource> soure)
        {
            var result=soure.Select(x => _mapper.Map<TDestination>(x)).ToList();
            return result;
        }

        public static TDestination MapList2(Tsource soure)
        {
            var result = _mapper.Map<TDestination>(soure);
            return result!;
        }

    }
}
