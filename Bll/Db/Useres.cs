using Dal;
using Dto;
using Dto.DataToObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Algorithm
{
    public class Useres
    {
        //user
        public string AddUser(UserDto user)
        {
            User_tbl u;
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                User_tbl u1 = db.User_tbl.Where(a => a.Phone == user.Phone).FirstOrDefault();
                if (u1 != null) return null;
                User_tbl u2 = user.DtoToDal();
                u = db.User_tbl.Add(u1);
                db.SaveChanges();
            }
            return u.Phone;
        }
        public void upDate(UserDto user)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                User_tbl user1 = db.User_tbl.First(a => a.Phone == user.Phone);
                user1.userName = user.userName;
                user1.businessName = user.businessName;
                user1.emailAddress = user.emailAddress;
                db.SaveChanges();
            }
        }
        public void delete(string phone)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                db.User_tbl.Remove(db.User_tbl.First(a => a.Phone == phone));
            }
        }
        public bool checkUser(string id)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                return db.User_tbl.Where(x => x.Phone == id).FirstOrDefault() != null ? true : false;
            }
        }

    }
}
