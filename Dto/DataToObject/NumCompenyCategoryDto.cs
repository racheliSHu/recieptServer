using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto.DataToObject
{
    public class NumCompenyCategoryDto
    {
     public NumCompenyCategoryDto()
        {

        }
        public NumCompenyCategoryDto (int caregory,int numCompeny,int count)
        {
            this.countShow = count;
            this.word = numCompeny;
            this.category = caregory;

        }
        public int ID { get; set; }
        public Nullable<int> category { get; set; }
        public Nullable<int> word { get; set; }
        public Nullable<int> countShow { get; set; }

        public static NumCompenyCategoryDto DalToDto(NumCompenyCategory_tbl numCompenyCategory)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<NumCompenyCategory_tbl, NumCompenyCategoryDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<NumCompenyCategoryDto>(numCompenyCategory);

        }
        public NumCompenyCategory_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<NumCompenyCategoryDto, NumCompenyCategory_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<NumCompenyCategory_tbl>(this);
        }
    }
}
