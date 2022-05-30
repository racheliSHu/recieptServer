using Bll.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OldUserController : ApiController
    {
        Useres Useres=new Useres();
        // GET: api/OldUser
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/OldUser/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OldUser
        public bool Post([FromBody] string value)
        {
            if (Useres.checkUser(value) == true)
                Global.UserID = value;
            return  Useres.checkUser(value); 
        }

        // PUT: api/OldUser/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OldUser/5
        public void Delete(int id)
        {
        }
    }
}
