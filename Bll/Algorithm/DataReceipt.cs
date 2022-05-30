using Dal;
using Dto;
using Dto.DataToObject;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto.DataToObject;
using Dto;
using System.Text.RegularExpressions;
using Bll.Db;

namespace Bll.Algorithm
{
    public class DataReceipt
    {
        Receipts receipt=new Receipts();
        public AllReceipt DataFromReceipt(List<List<string>> lines)
        {
            List<ProductsDto> products = new List<ProductsDto>();
            decimal totalSumReceipt = 0;
            string nameShop = null;
            string companyNumber = null;
            DateTime dateReceipt = new DateTime();
            List<string> wordForCareg = new List<string>();
            List<List<string>> part1 = new List<List<string>>();
            List<List<string>> part2 = new List<List<string>>();
            List<List<string>> part3 = new List<List<string>>();


            int i = 0;
            while (splite1(lines[i]))
            {
                part1.Add(lines[i]);
                if (i < lines.Count - 1)
                    i++;
            }
            if (i == lines.Count - 1)
                Console.WriteLine("try 2");
            List<string> s = lines[i];
            i++;
            while (splite2(lines[i]) || part2.Count < 1)
            {
                if (splite2(lines[i]) == false && part2.Count < 1 == true)
                    i++;
                if (i < lines.Count - 1)
                {
                    part2.Add(lines[i]);
                    i++;
                }
                else
                {
                    Console.WriteLine("LKJH");
                    break;
                }
            }
            while (i < lines.Count - 1)
            {
                if (i < lines.Count - 1)
                {
                    part3.Add(lines[i]);
                    i++;
                }
            }
            products = productTable(part2, s);

            //part 1
            foreach (var line in part1)
            {
                foreach (var word in line){ 
                    if (dateReceipt == new DateTime())
                        dateReceipt = isDateTime(word);
                }
                if (companyNumber == null || companyNumber == "")  companyNumber = analyzeCompanyNumber(line);
                if (nameShop == null)  nameShop = isNameShop(line);
            }
            decimal sumReceipt1 = 0;
            decimal sumReceipt = 0;
            foreach (var line in part3)
            {
                sumReceipt1 = getSumOfInvoice(line);
                foreach (var word in line)
                {
                    if (dateReceipt == new DateTime())
                        dateReceipt = isDateTime(word);
                }

                if (line.Any(word => word.Contains("לתשלום")))
                {
                    totalSumReceipt = analzeSumReceipt(line);
                }
                else
                {
                    if (totalSumReceipt == 0 && analzeSumReceipt(line) != 0)
                        sumReceipt = analzeSumReceipt(line);
                }
            }
            //nameShop = AnlyzeNameShop(part1);
            totalSumReceipt = totalSumReceipt != 0 ? totalSumReceipt : sumReceipt;
            ReceiptDto receipt = new ReceiptDto(dateReceipt, nameShop, companyNumber, (decimal)totalSumReceipt);
            //cheke if name shop null
            AllReceipt all = new AllReceipt(receipt, products);
            return all;
        }

        // פונקציה למציאת תאריך
        //פןנקציה שמקבלת מילה -סטרינג ומחיזרה תאריך אם המילה היא תאריך יוחזר תאריך אחרת ערך זבל
        public static DateTime isDateTime(string date)
        {
            var cultureInfo = new CultureInfo("de-DE");
            DateTime parsDate = new DateTime();
            if (string.IsNullOrEmpty(date)) return new DateTime();
            if (DateTime.TryParse(date, out DateTime dateTime))
                parsDate = DateTime.Parse(date, cultureInfo);
            return parsDate;
        }

        public string anlyzeNameProduct(List<string> nameprodact)
        {
            hashTableWordes h=new hashTableWordes();
            if (nameprodact == null) return null;
            if(nameprodact.Count()==1) return nameprodact.First();
            List<string> l1 = new List<string>();

            foreach (string word in nameprodact)
            {
                var w = word;
                if (h.searchWord(w)) w.Replace(w, "");
                foreach (char word2 in w) {
                    if(!IsLetterHebrow(word2)) w.Remove(word2);
                }
                if (w.Length > 1)
                    l1.Add(word);
            }

            return l1[0];

        }
    

        //פונקציה למציאת סכום כולל של החשבונית
        public static decimal analzeSumReceipt(List<string> line)
        {
            decimal sumReceipt = 0;
            string sumw = null;
            bool tryParse;
            if (line.Any(word => word.Contains("לתשלום") || word.Contains("סהכ")))
            {
                sumw = line.Where(word => word.Any(f => char.IsDigit(f))).LastOrDefault();
                if (sumw != null)
                {
                    sumw = String.Concat(sumw.Where(word => char.IsDigit(word) || word.Equals('.')));
                    tryParse = decimal.TryParse(sumw, out decimal d);
                    if (tryParse)
                    {
                        sumReceipt = decimal.Parse(sumw);
                    }
                    //else
                }
                return sumReceipt;
            }
            else
                return 0;

        }

        //פונקציה למציאת סכום כולל של החשבונית 
        private decimal getSumOfInvoice(List<string> lines)
        {
            string line = lines.Where(x => (x.Contains("לתשלום") || x.Contains("סהכ")|| x.Contains("שולמו")) && x.Any(y =>
            char.IsDigit(y))).LastOrDefault();
            if (line == null)
                line = lines.Where(x => x.Contains(" ₪") && x.Any(y =>
               char.IsDigit(y))).LastOrDefault();
            if (line == null)
                return 0;
            int lastIndex = line.LastIndexOf("סהכ");


            line = new string(line.Skip(lastIndex).ToArray());
            string sumStr = "";
            foreach (var item in line)

            {
                if (char.IsDigit(item) || item == '.')
                    sumStr += item.ToString();
            }
            return decimal.Parse(sumStr);
        }


        //פונקציה למציאת מספר חברה
        private static string analyzeCompanyNumber(List<string> line)
        {
            string companyNumber = null;
            bool b = line.Any(x =>
              x.Contains("ע.ר ") ||
              x.Contains("ער") ||
              x.Contains("ח.צ.") ||
              x.Contains(".ח.צ") ||
              x.Contains("מוסד") || x.Contains("ציבורי") ||
              x.Contains("עוסק") || x.Contains("פטור") ||
              x.Contains("עוסק") || x.Contains("מורשה") ||
              x.Contains("עוסק") || x.Contains("מס'") ||
              x.Contains("ח.פ") ||
              x.Contains(".ח.פ") ||
              x.Contains("ח.פ.") ||
              x.Contains("ער") ||
              x.Contains("ער.") ||
              x.Contains(".ער") ||
              x.Contains("מ.צ") ||
              x.Contains(".ע.ר") ||
              x.Contains("ע.ר.")
              );
            if (b)
                companyNumber = line.Where(d => d.Where(y => char.IsDigit(y)).Count() >= 7).FirstOrDefault();
            if (!b)
                companyNumber = line.Where(x => x.All(y => char.IsDigit(y))
                  && x.Length >= 7 &&
                 x.Length == 9).FirstOrDefault();
            if (companyNumber == null)
                return "";
            companyNumber = string.Concat(companyNumber.Where(x => x != '-'));
            List<string> splitcompany = companyNumber.Split(' ').ToList();
            if (splitcompany.Count == 1)
                return string.Concat(companyNumber.Where(x => char.IsDigit(x)).ToList());
            for (int i = 0; i < splitcompany.Count; i++)
            {
                splitcompany[i] = string.Concat(splitcompany[i].Where(x => char.IsDigit(x)).ToList());
            }
            splitcompany = splitcompany.Where(x => x != "").ToList();
            if (splitcompany.Count == 1)
                return splitcompany[0];
            return splitcompany.Where(x => x[0] != '0' && x.Length >= 7 && x.Length <= 10).FirstOrDefault();
        }

        //פןנקציה למציאת שם חנות 
        public static string isNameShop(List<string> line)
        {
            string nameShop = null;
            List<string> l1=new List<string>();
            foreach (var word in line)
            {
                if (word.Contains("בעמ") || "בעמ".Contains(word) || word.Equals(""))
                {
                    l1 = line.Where(w => w.Any(a => IsLetterHebrow(a) || char.IsLetter(a))).ToList();
                    if (l1 == null) return null;
                    foreach (string word2 in l1)
                        nameShop += word2 + " ";

                }
                else
                    return null;
            }
            return nameShop;
        }

        //חלוקת החשבונית בפעם הראשונה
        public static bool splite1(List<string> line)
        {
            List<string> spliteWord = new List<string>() { "קוד", "מקט", "לתשלום", "מחיר", "סכום", "פירוט", "תאור", "פריטים", "סהכ", "פרטים", "פריט", "כמות" };
            foreach (var word in line)
            {
                foreach (var word1 in spliteWord)
                {
                    if (word.Contains(word1) || word1.Contains(word) || word.Equals(word1)|| split1try2(line)==false)
                        return false;
                }
                return true;
            }
            return true;
        }
        public static bool split1try2(List<string> line)
        {
            List<Regex> regex1 = new List<Regex>();
            List<Regex> regex2 = new List<Regex>();
            string input = null;
            foreach (var word in line)
                input += word + " ";

            //                          sum     sum1  count  desc   code

            regex1.Add(new Regex(@"(\d+.\d+)\s+(\d+.\d+)\s+(\d+)\s+([ת - א] ||[a-z]+)\s+(\d+)"));
            regex1.Add(new Regex(@"(\d+.\d+)\s+([ת - א]||[a-z]+)\s+(\d+)"));
            regex1.Add(new Regex(@"(\d+.\d+)\s+(\d+.\d+)\s+([ת - א]||[a-z]+)\s+(\d+)"));//@"(\d+)\s+([-+*/])\s+(\d+)";
            regex1.Add(new Regex(@"(\d+.\d+)\s+(\d+.\d+)\s+(\d+)\s+([ת - א]||[a-z]+)"));
            regex1.Add(new Regex(@"(\d+.\d+)\s+(\d+)\s+([ת - א]||[a-z]+)\s+(\d+)"));

            //                          sum       sum1           count
            regex2.Add(new Regex(@" (\d+.\d+)(\d+.\d+)([*|x])(\d+.\d+)"));
            regex2.Add(new Regex(@"(\d+.\d+)(\d+.\d+)([*|x])(\d+)"));
            regex2.Add(new Regex(@"(\d+.\d+)([*|x])(\d+.\d+)"));
            foreach (var rgx in regex1)
            {
                if (rgx.Match(input).Success)
                    return false;
            }
            return true;
        }

        //חלוקת החשבונית בפעם השנייה

        public static bool splite2(List<string> line)
        {
            return !(line.Any(word => word.Contains("לתשלום") || word.Contains("סהכ") || word.Contains("סה") || word.Contains("%"))
                || line.Any(x => x.Contains("חייב")) && line.Any(s => s.Contains("מעמ")));
        }

        //חלק ראשון של החשבונית כלומר עד לטבלת המוצרים
        public static void part1(List<List<string>> lines)
        {





        }



       //פונקציה למציאת מוצר משורה בטבלת המוצרים

        public ProductsDto Product(List<string> line)
        {
            List<string> stringWord = new List<string>();
            string nameProduct = null;
            string costw = null;
            float cost = 0;
            string countw = null;
            int count = 1;
            if(line==null)
                return null;
            List<string> digitWord = new List<string>();
            List<string> spliteWord = new List<string>() { "ליחידה", "יחד", "לקילו", "לקג", "לגרם", "ליחד", "'יח" };

            for (int i = 0; i < line.Count-1; i++)
            {
                foreach (var word1 in spliteWord)
                {
                    if (line[i].Contains(word1) || word1.Contains(line[i]) || line[i].Equals(word1))
                        countw = line[i - 1];
                }

            }
            stringWord = line.Where(word => word.Any(a => IsLetterHebrow(a)==true || char.IsLetter(a))).ToList();
            //????????????
           // anlyzeNameProduct(stringWord);
            foreach (string word in stringWord)
                nameProduct += word + " ";
            digitWord = line.Where(word => word.Any(b => char.IsDigit(b))).ToList();
            costw = digitWord.Where(word => costOrCount(word) == 1).LastOrDefault();
            if (countw == null) countw = digitWord.Where(word => costOrCount(word) == 2).LastOrDefault();
            if (costw != null)
            {
                costw = String.Concat(costw.Where(word => char.IsDigit(word) || word.Equals('.')));
                cost = float.Parse(costw);
            }
            if (countw != null)
            {
                countw = String.Concat(countw.Where(word => char.IsDigit(word) || word.Equals('.')));

                count = int.Parse(countw);
            }
            ProductsDto prodact = new ProductsDto(nameProduct, count, cost);
            return prodact;
        }

        //פונקציה שמוצאת מוצר שכתוב בשני שורות ומחיזרה את הנונים מהשורה השנייה
        public ProductsDto productIn2(List<string> line)
        {
            List<string> spliteWord = new List<string>() { "ליחידה", "יחד", "לקילו", "לקג", "לגרם", "ליחד", "'יח" };
            string costw = null;
            float cost = 0;
            string countw = null;
            int count = 1;
            List<string> digitWord = new List<string>();

            for (int i = 0; i < line.Count; i++)
            {
                foreach (var word1 in spliteWord)
                {
                    if (line[i].Contains(word1) || word1.Contains(line[i]) || line[i].Equals(word1))
                        countw = line[i - 1];

                }

            }
            digitWord = line.Where(word => word.Any(b => char.IsDigit(b))).ToList();
            costw = digitWord.Where(word => costOrCount(word) == 1).LastOrDefault();
            while (line.Any(word => word.Contains("*") || word.Contains("x")))
                countw = line.Where(word => word.Any(b => char.IsDigit(b))).FirstOrDefault();
            if (costw != null)
            {
                costw = String.Concat(costw.Where(word => char.IsDigit(word) || word.Equals('.')));
                cost = float.Parse(costw);
            }
            if (countw != null)
            {
                countw = String.Concat(countw.Where(word => char.IsDigit(word)));
                count = int.Parse(countw);
            }
            ProductsDto prodact = new ProductsDto(null, count, cost);
            return prodact;

        }

        //חלק שני של החשבונית כלומר טבלת המוצרים
        public List<ProductsDto> productTable(List<List<string>> lines, List<string> line1)
        {
            bool flag = line1.Any(x => x.Contains("כמות"));

            List<ProductsDto> products = new List<ProductsDto>();
            ProductsDto product = new ProductsDto();
            ProductsDto product1 = new ProductsDto();
            for (int i = 0; i < lines.Count; i++)
            {
                if (!flag && i + 1 < lines.Count)
                {   //ch
                    if (lines[i + 1].Any(x => x.Contains("*") || x.Contains("x")))
                    {
                        product = Product(lines[i]);
                        product1 = productIn2(lines[i + 1]);
                        if (product.sumProduct == 0)
                            products.Add(new ProductsDto(product.nameProduct, (int)product1.amount, (float)product1.sumProduct));
                        else
                            products.Add(new ProductsDto(product.nameProduct, (int)product.amount, (float)product1.sumProduct));
                        i++;
                    }
                    else
                    {
                        product = Product(lines[i]);
                        if (product.sumProduct == 0)
                        {
                            product1 = productIn2(lines[i + 1]);
                            products.Add(new ProductsDto(product.nameProduct + product1.nameProduct, (int)product1.amount, (float)product1.sumProduct));
                            i++;
                        }
                        else
                            products.Add(product);
                    }
                }
                else
                {
                    product = Product(lines[i]);
                    products.Add(product);
                }
            }
            return products;
        }

        //בדיקה האם המספר בשורה שייך לכמןת או למחיר
        public int costOrCount(string word)
        {
            decimal cost = 0;
            int count = 0;
            string costWord = null;
            bool a1 = false;

            if (word == null)
                return 0;
            if (word.Contains("₪") || word.Contains("ש''ח"))
                return 1;
            if (word.Contains("."))
            {
                costWord = String.Concat(word.Where(w => char.IsDigit(w) || w.Equals('.')));
                cost = decimal.Parse(costWord);
                if (cost % 1 != 0)
                    return 1;
                return 1;
            }
            else
            {
                if (int.TryParse(word, out count))
                    count = int.Parse(word);
                else
                    return 0;
                if (count > 50 || word[0] == 0)
                    return 0;
                else
                    return 2;
            }
            return 0;


        }
        //חלק שלישי של החשבונית כלומר לאחר טבלת המוצרים
        public static void part3(string word)
        {

        }
        //  לבדיקה ובודק אם הוא אות עברית CHAR מקבל
        public static bool IsLetterHebrow(char c)
        {
            if (c >= 'א')
            {
                return c <= 'ת';
            }

            return false;
        }
        public string AnlyzeNameShop(List<List<string>> lines)
        {
            hashTableWordes hash=new hashTableWordes();
            hash.initTableWordes();
            string nameShop = null;
            List<string> listWordes = new List<string>();
            foreach (List<string> line in lines)
            {
                foreach (string w in line)
                {
                    var word = Regex.Replace(w, @"[\d-]", string.Empty);
                    new List<string> { ":", "@", ",", ";", "*", "+", "=", "'", "[", "]", "(", ")", "\"", "!", " .", "/" }.ForEach(e => word = word.Replace(e, " "));
                    if(!hash.searchWord(word) && word.Length > 1)
                        listWordes.Add(word);
                }
            }
            
            return nameShop;
        }
    }



}







