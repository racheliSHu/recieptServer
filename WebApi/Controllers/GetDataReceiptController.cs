using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bll.Algorithm;
using Bll.Db;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GetDataReceiptController : ApiController
    {
        Receipts receipts=new Receipts();

        // GET: api/GetDataReceipt
        public List<AllReceipt> Get()
        {
            return receipts.getAllReceipt();
        }
        //click on enter

        // GET: api/GetDataReceipt/5
      



        // POST: api/GetDataReceipt
        public void Post(string value)
        {
        }

        // PUT: api/GetDataReceipt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GetDataReceipt/5
        public void Delete(int id)
        {
        }
    }
}
