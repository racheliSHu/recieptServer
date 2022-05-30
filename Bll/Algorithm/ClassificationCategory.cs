using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dal;
using Dto.DataToObject;

namespace Bll.Algorithm
{
    public class ClassificationCategory
    {

        Category Category = new Category();
        ////רשימה של כל המילים במסד הנתונים
        private List<WordCategoryDto> lw = new List<WordCategoryDto>();
        ////רשימה של כל המילים  שמשויכות לקטגוריה מסוימת במסד הנתונים
        private List<WordPerCategoryDto> lwc = new List<WordPerCategoryDto>();
        ////רשימה של כל הקטגוריות במסד הנתונים
        private List<CategoryDto> lc = new List<CategoryDto>();
        //
        private List<NumCompenyDto> numCompenies = new List<NumCompenyDto>();
        //
        private List<NumCompenyCategoryDto> numCompenyCategories = new List<NumCompenyCategoryDto>();

        //
        private List<UnusingWord_tbl> un = new List<UnusingWord_tbl>();
        hashTableWordes h = new hashTableWordes();
        //
        private float[,] staticMatrixs;
        private float[,] compenyMatrixs;
        private int sumOfAllWord = 0;

        //הפונקציה מאתחלת את כל הרשימות מהטבלאות בהמסד נתונים 
        //וכן בונה את המטריצות  
        public void initAllListAndMatrix()
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var item in db.Category_tbl)
                {
                    lc.Add(CategoryDto.DalToDto(item));
                }
                foreach (var item in db.WordCategory_tbl)
                {
                    lw.Add(WordCategoryDto.DalToDto(item));
                }
                foreach (var item in db.WordPerCategory_tbl)
                {
                    lwc.Add(WordPerCategoryDto.DalToDto(item));
                }
                foreach (var item in db.NumCompeny_tbl)
                {
                    numCompenies.Add(NumCompenyDto.DalToDto(item));
                }
                foreach (var item in db.NumCompenyCategory_tbl)
                {
                    numCompenyCategories.Add(NumCompenyCategoryDto.DalToDto(item));
                }
            }
            
            staticMatrixs = new float[lc.Count , lw.Count + 1];
            compenyMatrixs = new float[lc.Count , numCompenies.Count];
            foreach (var item in lc)
            {
                int count = lwc.Where(a => a.category == item.Id).Count();
                staticMatrixs[lc.IndexOf(item), lw.Count] = count !=0 ?count:1;
                sumOfAllWord += count;
            }
            foreach (var item in lwc)
            {
                CategoryDto c = lc.Where(x => x.Id == item.category).FirstOrDefault();
                WordCategoryDto word = lw.Where(x => x.Id == item.word).FirstOrDefault();
                int x1 =(int) item.countWord;
                float v = staticMatrixs[lc.IndexOf(c), lw.Count];
                float count= x1/v;

                staticMatrixs[lc.IndexOf((CategoryDto)c), lw.IndexOf((WordCategoryDto)word)] = count;
                    
            }
            foreach (var item in numCompenyCategories)
            {
                CategoryDto c = lc.Where(x => x.Id == item.category).FirstOrDefault();
                NumCompenyDto num = numCompenies.Where(x => x.ID == item.word).FirstOrDefault();
                compenyMatrixs[lc.IndexOf(c), numCompenies.IndexOf(num)] = (float)item.countShow;
            }

        }

        //הפונקציה מקבלת מילה 
        //ומחזירה את המילים הדומות לה ועחוז הדמיון
        public Dictionary<string, float> getSimiliarWords(string word)
        {
            string htmlCode;
            Dictionary<string, float> similarWords = new Dictionary<string, float>();
            try
            {
                using (WebClient client = new WebClient())
                {
                    var htmlData = client.DownloadData(" https://u.cs.biu.ac.il/~yogo/tw2v/similar/" + word);
                    htmlCode = Encoding.UTF8.GetString(htmlData);

                }
                string[] s = htmlCode.Split(new string[] { "/td" }, StringSplitOptions.None);
                if (s[0].Contains("<xxx>x") == true)
                    return null;
                for (int z = 0, i = 3; z < 10; z++)
                {
                    int startNl = s[i].IndexOf("color");
                    int startN = s[i].IndexOf(">", startNl);
                    startN++;
                    int endN = s[i].IndexOf("<", startN);
                    float percent = (float.Parse)(s[i].Substring(startN, endN - startN));
                    int start = s[i + 1].IndexOf("similar/");
                    start += 8;
                    int end = s[i + 1].IndexOf("'", start);
                    string wordd = s[i + 1].Substring(start, end - start);
                    i += 3;
                    similarWords.Add(wordd, percent);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return similarWords;
        }


        //הפונקציה מקבלת רשימה של מילים 
        //הפונקציה מחזירה מילון שכל ערך בו מכיל מילה ומילון של כל המילים הדומות למילה
        //סהכ מוחזר לי כל המילים והמילים הדומות להם
        //הפונקציה שולחת כל מילה לפונקציה  של מילים דומות באופן אסינכרוני המקצר את זמן הריצה
        private Dictionary<string, Dictionary<string, float>> getAllSimiliarWords(List<string> words)
        {
            Dictionary<string, Dictionary<string, float>> allSimiliarWords = new Dictionary<string, Dictionary<string, float>>();
            List<Task<(string, Dictionary<string, float>)>> tasks = new List<Task<(string, Dictionary<string, float>)>>();
            foreach (string word1 in words)
            { //עובר על כל מילה/מוצר מהחשבונית ומחזיר רשימה של כל המילים הדומות לה ואחוז הדמיון
                Task<(string, Dictionary<string, float>)> task = Task.Run(() =>
                {
                    return (word1, getSimiliarWords(word1));
                });

                tasks.Add(task);
            }
            foreach (var task in tasks)
            {
                task.Wait();
                (string word, Dictionary<string, float> similiarWords) = task.Result;
                allSimiliarWords.Add(word, similiarWords);
            }
            return allSimiliarWords;
        }



        //הפונקציה מקבלת מילים דומות למילה חדשה וקטגוריה  
        //ממוצע משוקלל -מחזירה ערך של הסתברות לקטגוריה זו
        //עי חישוב עבור כל מילה מחשבים 
        //אחוז הקרבה למילה*ההסתברות שהמילה שיכת לקטגוריה 
        //ומחלקים בסך כל אחוזי הדמיון
        public double staticSimiliarWords(Dictionary<string, float> similiarWords, int category1)
        {
            float calc =0;
            float sum=0;
            if (similiarWords == null)
                return 0;
            foreach (var item in similiarWords)
            {
                int n = lw.Where(x => x.categoryWord == item.Key).Select(y => y.Id).FirstOrDefault();
                WordPerCategoryDto cpw = lwc.Where(x => x.category == category1 && x.word == n).Select(y => y).FirstOrDefault();
                if (cpw != null)
                {
                    CategoryDto category = (CategoryDto)lc.Where(x => x.Id == cpw.category).FirstOrDefault();
                    WordCategoryDto word = lw.Where(x => x.Id ==cpw.word).FirstOrDefault();
                    sum += item.Value / 100;
                    float c = staticMatrixs[lc.IndexOf(category), lw.IndexOf(word)];
                    calc += c * (item.Value / 100);
                   
                }
            }
            return calc/sum;
        }

        //הפונקציה הראשית מקבלת רשימה של מוצרים ומספר עסק 
        //מחזירה מילון של הקטגוריות וההסתברות שלהן להתאמה
        public Dictionary<int, double> newReceipt(string content, string numCompany)
        {
            
            initAllListAndMatrix();
            Dictionary<int, double> staticsCategory = new Dictionary<int, double>();
            List<string> allWords = stringToWordes(content);
            Category.addWordToDb(allWords);
            Category.addCompenyToDb(numCompany);
            staticsCategory = ClassificatioCompany(numCompany);
            if (staticsCategory == null)
                staticsCategory = ClassificatioContent(content);

            staticsCategory = staticsCategory.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
            int c = staticsCategory.ElementAt(0).Key;
            Category.addCountToAllWordes(allWords, c);
            Category.AddCountCompeny(numCompany, c);

            return staticsCategory;

        }

        //סיווג לפי מספר עסק 
        //מקבלת מספר עסק ולפי המטריצה שבניתי
        //מחזירה מילון של הקטגוריות וההסתברות שלהן להתאמה

        public Dictionary<int, double> ClassificatioCompany(string numCompany)
        {
            NumCompenyDto numCompeny1 = numCompenies.Where(a => a.numCompany == numCompany).FirstOrDefault();
            Dictionary<int, double> staticsCategory = new Dictionary<int, double>();
            int count;
            if (numCompany == null || numCompany == "" || numCompeny1 == null)
                return null;

            foreach (var c in lc)
            {
                NumCompenyDto num = numCompenies.Where(x => x.numCompany == numCompany).FirstOrDefault();
                count = (int)compenyMatrixs[lc.IndexOf(c), numCompenies.IndexOf((NumCompenyDto)num)];
                staticsCategory.Add(c.Id, count);
            }
            return staticsCategory;
        }


        //סיווג לפי רשימת המוצרים
        //בודקת התאמה לכל קטגוריה בצורה הסתברותית
        //מחזירה מילון של הקטגוריות וההסתברות שלהן להתאמה
        public Dictionary<int, double> ClassificatioContent(string content)
        {
            List<string> allWords = stringToWordes(content);
            //מילון שמכיל את כל מילות החשבונית והמילים הדומות להם
            Dictionary<string, Dictionary<string, float>> similiarWords = getAllSimiliarWords(allWords);
            Dictionary<int, double> staticsCategory = new Dictionary<int, double>();
            //לכל קטגוריה חשבתי את ההסתברות לקטגוריה זו 
            //ההסתברות-סכום כל המופעים של הקטגוריה חלקי סכום כל המופעים בכל הקטגוריות
            for (int i = 0; i < lc.Count; i++)
            {
                float calc = staticMatrixs[lc.IndexOf(lc[i]), lw.Count]/sumOfAllWord;
                staticsCategory.Add(lc[i].Id, calc!=0?calc:0.001);
            }
            foreach (var word in allWords)
            {
                int flag = -1;
                double s = 0;
                double category = 0;
                //בדיקה האם המילה שבחשבונית שאנו עוברים עליה כעת נמצאת במסד הנתונים
                WordCategoryDto w = lw.Where(x => x.categoryWord == word).Select(y => y).FirstOrDefault();
                if (w != null)
                {
                    int index = w.Id;
                    flag = index;
                }
                foreach (var category1 in lc)
                {
                    if (flag >= 0)
                    {
                        WordCategoryDto w1 =lw.Where(x=> x.Id == flag).Select(y => y).FirstOrDefault();
                        s = staticMatrixs[lc.IndexOf(category1), lw.IndexOf(w1)];
                    }
                    if (s == 0)
                        // אם למילה אין מופע לקטגוריה זו בודקים את ההתאמה לקטגוריה זו ע"י מילים דומות
                        category = staticSimiliarWords(similiarWords[word], category1.Id);
                    staticsCategory[category1.Id] *= s > 0 ? s : category > 0 ? category : 0.001;
                }
            }
            return staticsCategory;
        }


        //ממירה מחרוזת למילים ומנקה תווים מיותרים
        //מקבלת מחרוזת
        //מחזירה רשימה של מילים
        public List<string> stringToWordes(string text)
        {
            if (text == "") return null;
            char[] splites = new char[] { '\t', '\n', '\r', ' ' };
            List<string> wordes = text.Split(splites).ToList();
            List<string> listWordes = new List<string>();
            foreach (var w in wordes)
            {
                var word = Regex.Replace(w, @"[\d-]", string.Empty);
                new List<string> { ":", "@", ",", ";", "*", "+", "=", "'", "[", "]", "(", ")", "\"", "!", " .", "/" }.
                    ForEach(e => word = word.Replace(e, ""));
               
                if (word.Length > 1 && word.All(c => IsLetterHebrow(c)))
                    listWordes.Add(word);
            }
            return listWordes;

        }
        public bool IsLetterHebrow(char c)
        {
            if (c >= 'א')
            {
                return c <= 'ת';
            }

            return false;
        }


    }


}



