using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Bll.Algorithm;
using Dto.DataToObject;
using Bll.Db;
using Bll;
using Newtonsoft.Json.Linq;
using Dto;

namespace WebApi.Controllers

{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class tryController : ApiController
    {
        StartAlgoritem algorithem12 = new StartAlgoritem();
        ClassificationCategory c = new ClassificationCategory();

        Category c1 = new Category();
        Receipts r1 = new Receipts();
        Program1 program = new Program1();
        Product product = new Product();
        Program program11=new Program();
        hashTableWordes  h=new hashTableWordes();
        Useres useres = new Useres();   
        // GET: api/try
        public IEnumerable<string> Get()
        {
            Global.UserID = "0556779998";
            Dictionary<string, decimal> d= r1.getAllCategorySum();

            List<AllReceipt> l11 = r1.getAllReceiptPerCategory("מזון");
            if (useres.checkUser("0556779998") == true)
                Global.UserID = "0556779998";

            Console.WriteLine(Global.UserID);



            //List<string> words=new List<string>();
            //if (c.getSimiliarWords("תיק") != null)
            //{
            //    foreach (var item in c.getSimiliarWords("תיק"))
            //        words.Add(item.Key);
            //}
            //c1.addWordToDb(words);
            //c1.addCountToAllWordes(words,1);
            //dbClass.tryfunc();
            // c1.AddCount(43, 8);
            // c.newReceipt(" צנצנת שולחן כסא ספה", "");

            //bool b = DataReceipt.split1try2(new List<string>() { "12.2", "rjkk", "124" });
            //  c.getSimiliarWords("רופא");

            string path = "C:/racheli/Try/Try/bin/Debug/20.jpg";
            AllReceipt receipts = algorithem12.AnlyzeReceipt(path);
            //List<string> list1 = new List<string>();
            //Dictionary<string, int> dic = new Dictionary<string, int>();
            //string list = null;
            //foreach (var product in receipts.products)
            //    list += product.nameProduct + " ";
            //if (list != null)
            //    dic = c.newReceipt(list);
            //if (dic == null)
            //    Console.WriteLine("kjhgf");
            //foreach (var item in dic)//
            //    list1.Add(item.Key);
            //return list1;

            return new string[] { "value1", "value2" };

        }

        // GET: api/try/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/try
        public void Post(object user)
        {
            Dictionary<string, float> dic1 = new Dictionary<string, float>();
        }

        // PUT: api/try/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/try/5
        public void Delete(int id)
        {
        }
        //פונקציה בדיקת תאריך
        //public DateTime isDateTime(string date)
        //{
        //    var cultureInfo = new CultureInfo("de-DE");
        //    DateTime parsDate = new DateTime();
        //    //??????
        //    if (string.IsNullOrEmpty(date)) return new DateTime();

        //    if (DateTime.TryParse(date, out DateTime dateTime))

        //        parsDate = DateTime.Parse(date, cultureInfo);
        //    Console.WriteLine(parsDate);
        //    return parsDate;
        //}
    }
}
