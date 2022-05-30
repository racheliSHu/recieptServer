using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto.DataToObject
{
    public class UnusingWordDto
    {
        public UnusingWordDto()
        {
                
        }
        public UnusingWordDto(string w)
        {
            this.unusingWord = w;
        }
        public int Id { get; set; }
        public string unusingWord { get; set; }
        public static UnusingWordDto DalToDto(UnusingWord_tbl unusingWord)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<UnusingWord_tbl, UnusingWordDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<UnusingWordDto>(unusingWord);

        }
        public UnusingWord_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<UnusingWordDto, UnusingWord_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<UnusingWord_tbl>(this);
        }
    }
}
