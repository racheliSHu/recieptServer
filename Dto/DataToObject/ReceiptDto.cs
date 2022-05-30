using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using AutoMapper;

namespace Dto.DataToObject
{
    public class ReceiptDto
    {
        public ReceiptDto()
        {
                
        }
        public ReceiptDto(DateTime datereceipt, string nameShop,string numcompeny,decimal sum)
        {
            this.nameShop = nameShop;
            this.totalSum = sum;
            this.numCompany = numcompeny;
            this.dateReceipt= datereceipt;
            
        }
        public int Id { get; set; }
        public Nullable<System.DateTime> dateReceipt { get; set; }
        public string nameShop { get; set; }
        public string numCompany { get; set; }
        public Nullable<decimal> totalSum { get; set; }
        public string myUser { get; set; }
        public Nullable<int> category { get; set; }
        public string path { get; set; }    


        public static ReceiptDto DalToDto(Receipt_tbl receipt)
        {
            var config = new MapperConfiguration(cfg =>
                 cfg.CreateMap<Receipt_tbl, ReceiptDto>()
             );
            var mapper = new Mapper(config);
            return mapper.Map<ReceiptDto>(receipt);

        }
        public Receipt_tbl DtoToDal()
        {
            var config = new MapperConfiguration(cfg =>
                     cfg.CreateMap<ReceiptDto, Receipt_tbl>()
                 );
            var mapper = new Mapper(config);
            return mapper.Map<Receipt_tbl>(this);
        }
    }
}
