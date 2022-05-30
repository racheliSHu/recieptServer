using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;

namespace Dto.DataToObject
{
    public class WordCategoryDto
    {
        public WordCategoryDto()
        {

        }
        public WordCategoryDto(string w)
        {
            this.categoryWord = w;
        }
        public int Id { get; set; }
        public string categoryWord { get; set; }

        public static WordCategoryDto DalToDto(WordCategory_tbl word)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<WordCategory_tbl, WordCategoryDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<WordCategoryDto>(word);

        }
        public WordCategory_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<WordCategoryDto, WordCategory_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<WordCategory_tbl>(this);
        }



    }
}
