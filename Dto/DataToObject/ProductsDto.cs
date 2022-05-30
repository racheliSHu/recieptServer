using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dal;

namespace Dto.DataToObject
{
    public class ProductsDto
    {
        public ProductsDto()
        {

        }
        public ProductsDto(string name,int count,float sum)
        {
            this.sumProduct = sum;
            this.amount = count;
            this.nameProduct = name;
        }
        public int Id { get; set; }
        public string nameProduct { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<float> sumProduct { get; set; }
        public Nullable<int> receipt { get; set; }

        public static ProductsDto DalToDto(Products_tbl product)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Products_tbl, ProductsDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ProductsDto>(product);

        }
        public Products_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<ProductsDto, Products_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<Products_tbl>(this);
        }

    }
}
