using Dal;
using Dto.DataToObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Algorithm
{
    public class hashTableWordes
    {
        Receipts_dbEntities1 db = new Receipts_dbEntities1();
        //??????????
        private HashSet<string> tableWordes = new HashSet<string>();
        

        public bool searchWord(string word) {
            return tableWordes.Contains(word);
        }
        public void insertWord(string word)
        {
            UnusingWordDto un=new UnusingWordDto();
            if (!searchWord(word))
            {
                un = new UnusingWordDto(word);
                tableWordes.Add(word);
                //add to data bace without id
                db.UnusingWord_tbl.Add(un.DtoToDal());
                db.SaveChanges();

            }
            //update data bace 
        }
        public void initTableWordes()
        {

            foreach (var item in db.UnusingWord_tbl)
            {
                tableWordes.Add(item.unusingWord);
            }
            
        }
    }
}

