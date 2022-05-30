using Dal;
using Dto;
using Dto.DataToObject;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Algorithm
{
    public class Category
    {
        public void AddWord(string word, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                WordCategoryDto w1 = WordCategoryDto.DalToDto(db.WordCategory_tbl.Where(x => x.categoryWord == word).Select(y => y).FirstOrDefault());
                if (w1 == null)
                {
                    WordCategoryDto wordCategoryDto = new WordCategoryDto(word);
                    WordCategory_tbl w = db.WordCategory_tbl.Add(wordCategoryDto.DtoToDal());
                    WordPerCategoryDto wordper = new WordPerCategoryDto(category, w.Id, 0);
                    db.WordPerCategory_tbl.Add(wordper.DtoToDal());
                    db.SaveChanges();
                }
                else
                    return;
            }

        }
        public void addWordPerCategory(int word, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                WordPerCategoryDto wordper = new WordPerCategoryDto(category, word, 0);
                db.WordPerCategory_tbl.Add(wordper.DtoToDal());
                db.SaveChanges();
            }

        }
        public WordPerCategoryDto GetWordPerCategoryDto(int word, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                return WordPerCategoryDto.DalToDto(db.WordPerCategory_tbl.FirstOrDefault(x => x.category == category && x.word == word));
            }
        }
        public void AddCount(int word, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                if (GetWordPerCategoryDto(word, category) == null)
                    addWordPerCategory(word, category);
                WordPerCategory_tbl w = db.WordPerCategory_tbl.FirstOrDefault(x => x.category == category && x.word == word);
                w.countWord+=10;
                db.SaveChanges();
            }
        }
        public void addWordToDb(List<string> wordes)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (string word in wordes)
                {
                    foreach (var category in db.Category_tbl)
                    {
                        AddWord(word, category.Id);
                    }
                }
            }
        }
        public void addCountToAllWordes(List<string> wordes, int c)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var word in wordes)
                {
                    WordCategory_tbl w= db.WordCategory_tbl.Where(x => x.categoryWord == word).FirstOrDefault();
                    
                    if (w!= null)
                        AddCount(w.Id, c);
                    else
                        AddWord(word, c);
                }
            }
        }
        public void decreaseCountWord(string words, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                int word = db.WordCategory_tbl.Where(a => a.categoryWord == words).Select(b => b.Id).FirstOrDefault();
                if (GetWordPerCategoryDto(word, category) == null)
                    addWordPerCategory(word, category);
                WordPerCategory_tbl w = db.WordPerCategory_tbl.FirstOrDefault(x => x.category == category && x.word == word);
                w.countWord--;
                db.SaveChanges();
            }
        }




        //compeny


        public void AddCompeny(string compeny, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                NumCompeny_tbl num = db.NumCompeny_tbl.Where(a => a.numCompany == compeny).FirstOrDefault();
                if (num==null)
                {
                    NumCompenyDto numCompeny = new NumCompenyDto(compeny);
                    NumCompeny_tbl numCompeny_tbl = db.NumCompeny_tbl.Add(numCompeny.DtoToDal());
                    NumCompenyCategoryDto numCompenyCategory = new NumCompenyCategoryDto(category, numCompeny_tbl.ID, 0);
                    db.NumCompenyCategory_tbl.Add(numCompenyCategory.DtoToDal());
                    db.SaveChanges();
                }
                else
                    addNumCompanyCategory(num.ID, category);
            }
        }
        private void addNumCompanyCategory(int numCompany, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                NumCompenyCategory_tbl num= db.NumCompenyCategory_tbl.Where(a=>a.word==numCompany&&a.category==category).FirstOrDefault();
                if (num == null)
                {
                    NumCompenyCategoryDto numCompenyCategory = new NumCompenyCategoryDto(category, numCompany, 0);
                    db.NumCompenyCategory_tbl.Add(numCompenyCategory.DtoToDal());
                    db.SaveChanges();
                }
            }

        }
        public NumCompenyCategoryDto GetNumCompanyCategory(int numCompany, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                return NumCompenyCategoryDto.DalToDto(db.NumCompenyCategory_tbl.FirstOrDefault(x => x.word == numCompany && x.category == category));
            }
        }
        public void AddCountCompeny(string numCompanyS, int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                int numCompany = db.NumCompeny_tbl.Where(a => a.numCompany==numCompanyS).Select(b => b.ID).FirstOrDefault();
                if (numCompany < 0)
                    addCompenyToDb(numCompanyS);
                if (GetNumCompanyCategory(numCompany, category) == null)
                    addNumCompanyCategory(numCompany, category);
                NumCompenyCategory_tbl numCompeny = db.NumCompenyCategory_tbl.Where(x => x.word == numCompany && x.category == category).FirstOrDefault();

                numCompeny.countShow+=10;
                db.SaveChanges();
            }
        }
        public void addCompenyToDb(string numcomeny)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var category in db.Category_tbl)
                {
                    AddCompeny(numcomeny, category.Id);
                }
            }
        }

        public void updateCountWord(List<string> wordes, int cAdd, int cDec)
        {
            addCountToAllWordes(wordes, cAdd);
            foreach (var worde in wordes)
            {
                decreaseCountWord(worde, cDec);
            }
        }


        public List<int> tryfunc()
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                List<int> list = new List<int>();
                List<WordPerCategory_tbl> lwc = new List<WordPerCategory_tbl>();
                //List<WordPerCategory_tbl> r=db.WordPerCategory_tbl.Where(x => x.word == 433||x.word==434
                //|| x.word == 440 || x.word == 436 || x.word == 442 || x.word == 444 || x.word == 446
                // || x.word == 448||x.word==338 
                //).ToList();
                // foreach (WordPerCategory_tbl w in r)
                //     db.WordPerCategory_tbl.Remove(w);
                // List<WordCategory_tbl> g=db.WordCategory_tbl.Where(x => x.Id == 433 || x.Id == 434
                //|| x.Id == 440 || x.Id == 436 || x.Id == 442 || x.Id == 444 || x.Id == 446
                // || x.Id == 448||x.Id==338
                //).ToList();
                // foreach(WordCategory_tbl w in g)
                //     db.WordCategory_tbl.Remove(w);
                foreach (var item in db.WordPerCategory_tbl)
                {
                    lwc.Add(item);
                }
                foreach (var item in lwc)
                {
                    WordPerCategory_tbl w = item;
                    w.countWord = 10;
                    db.SaveChanges();
                }

                list = db.WordPerCategory_tbl.Where(x => x.category == 8).Select(y => (int)y.word).ToList();
                return list;
            }


        }
       

    }

}

