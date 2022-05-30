using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bll.Algorithm;
using Bll.Db;
using Dto;
using Dto.DataToObject;
namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SaveReceiptController : ApiController
    {
        Receipts receipt = new Receipts();

        public int idReceipt { get; set; }
        // GET: api/SaveReceipt
        public IEnumerable<string> Get()
        {  
          
            return new string[] { "value1", "value2" };
        }

        // GET: api/SaveReceipt/5
        public string Get(string idUser)
        {
           return "value";  
        }
        //לשלוח כולל הניתוב שנשלח
        // POST: api/SaveReceipt
        public void Post(AllReceipt receipts)
        {
            receipt.addAllReceipt(receipts);
        }

        //צריכה לקבל את שני הקטגוריות או אמת או שקר אם השתנה
        // PUT: api/SaveReceipt/5
        public void Put(int id)
        {
           
        }

        // DELETE: api/SaveReceipt/5
        public void Delete(int id)
        {
        }
    }
}
