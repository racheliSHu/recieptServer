using Bll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Bll.Algorithm;
using Bll.Db;

namespace WebApi.Controllers
{

    public class CaregoryController : ApiController
    {
        Receipts r;
        // GET: api/Caregory
        public Dictionary<string, decimal> Get()
        {
           return r.getAllCategorySum();
        }

        // GET: api/Caregory/5

        //click on category receipt
        // POST: api/Caregory
        public List<AllReceipt> Post(string category)
        {
            return r.getAllReceiptPerCategory(category);
        }

        // PUT: api/Caregory/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Caregory/5
        public void Delete(int id)
        {
        }
    }
}
