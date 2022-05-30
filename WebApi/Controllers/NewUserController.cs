using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dto;
using Dto.DataToObject;
using Bll.Algorithm;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NewUserController : ApiController
    {
      
        Useres useres=new Useres();
        public static string userId;
        // GET: api/NewUser
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //user lockal
        // GET: api/NewUser/5
        public void Get(string  id)
        { 
           
        }
        //new user
        // POST: api/NewUser
        public void Post([FromBody]UserDto user)
        {
            string id=useres.AddUser(user);
            Global.UserID = id; 
        }

        // PUT: api/NewUser/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/NewUser/5
        public void Delete(int id)
        {
        }
    }
}
