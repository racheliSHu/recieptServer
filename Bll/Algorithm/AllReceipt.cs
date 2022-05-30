using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Dto.DataToObject;

namespace Bll.Algorithm
{
    public class AllReceipt
    {
        public ReceiptDto receipt { get; set; }
        public List<ProductsDto> products { get; set; }

        public AllReceipt()
        {
            this.receipt = new ReceiptDto();
            this.products = new List<ProductsDto>();
        }
        public AllReceipt(ReceiptDto r, List<ProductsDto> p)
        {
            this.receipt = r;
            this.products = p;
        }


     
    }
}
