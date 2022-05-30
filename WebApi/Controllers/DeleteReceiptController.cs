using Bll.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class DeleteReceiptController : ApiController
    {
        Receipts receipt = new Receipts();
       
        // GET: api/DeleteReceipt
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/DeleteReceipt/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DeleteReceipt
        public void Post(int idReceipt)
        {
            receipt.deleteReceipt(idReceipt);
        }

        // PUT: api/DeleteReceipt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DeleteReceipt/5
        public void Delete(int id)
        {
        }
    }
}
