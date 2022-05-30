using Bll.Algorithm;
using Bll.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    
    public class UpDateController : ApiController
    {
        Receipts receipt = new Receipts();
        // GET: api/UpDate
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/UpDate/5
        public string Get(int id)
        {
            return "value";
        }
        //משמש לעדכון כולל עדכון קטגוריה ומקבל בפרמטר השני את הקטגוריה הישנה
        // POST: api/UpDate
        public void Post(AllReceipt receipts,int category)
        {
            receipt.upDateReceipt(receipts,category);
        }

        // PUT: api/UpDate/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UpDate/5
        public void Delete(int id)
        {
        }
    }
}
