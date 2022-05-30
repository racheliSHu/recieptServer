using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;


namespace Dto.DataToObject
{
    public class CategoryDto
    {
        public CategoryDto()
        {
                
        }
        public int Id { get; set; }
        public string nameCategory { get; set; }


        public static CategoryDto DalToDto(Category_tbl category)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Category_tbl, CategoryDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<CategoryDto>(category);

        }
        public Category_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<CategoryDto, Category_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<Category_tbl>(this);
        }
    }
}
