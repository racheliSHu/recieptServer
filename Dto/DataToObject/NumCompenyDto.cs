using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dal;
using AutoMapper;

namespace Dto.DataToObject
{
    public class NumCompenyDto
    {
        public NumCompenyDto()
        {

        }
        public NumCompenyDto(string numCompeny)
        {
            this.numCompany = numCompeny;
        }
        public int ID { get; set; }
        public string numCompany { get; set; }


        public static NumCompenyDto DalToDto(NumCompeny_tbl NumCompeny)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<NumCompeny_tbl, NumCompenyDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<NumCompenyDto>(NumCompeny);

        }
        public NumCompeny_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<NumCompenyDto, NumCompeny_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<NumCompeny_tbl>(this);
        }
    }
}
