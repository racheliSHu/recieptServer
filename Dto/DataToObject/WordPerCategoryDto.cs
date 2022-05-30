using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;

namespace Dto.DataToObject
{
    public class WordPerCategoryDto
    {
        public WordPerCategoryDto()
        { 

        }
        public WordPerCategoryDto(int category, int word,int count)
        {
            this.category = category;
            this.word = word;
            this.countWord = count;
        }
        public int ID { get; set; }
        public Nullable<int> category { get; set; }
        public Nullable<int> word { get; set; }

        public Nullable<int> countWord { get; set; }

        public static WordPerCategoryDto DalToDto(WordPerCategory_tbl word)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<WordPerCategory_tbl, WordPerCategoryDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<WordPerCategoryDto>(word);

        }
        public WordPerCategory_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<WordPerCategoryDto, WordPerCategory_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<WordPerCategory_tbl>(this);
        }
    }
}
