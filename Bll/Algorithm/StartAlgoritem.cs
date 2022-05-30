using Bll.Db;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bll.Algorithm
{
   public  class StartAlgoritem
    {
        Category category=new Category();
        Ocr ocr=new Ocr();
        DataReceipt DataReceipt = new DataReceipt();
        ClassificationCategory classificationCategory = new ClassificationCategory();
        List<List<string>> l1 = new List<List<string>>();
        Dictionary<int, double> dict = new Dictionary<int, double>();
        List<string> l2 = new List<string>();
        Receipts receipt = new Receipts();
        string list = null;
        hashTableWordes h = new hashTableWordes();
        public AllReceipt AnlyzeReceipt(string path)
        {
            Global.UserID = "0556779998";

            //פיענוח וחלוקה לשורות OCR-שליחה ל
            l1 =ocr. getTextFromPath(path);
            //חילוץ נתוני החשבונית
            AllReceipt receipts= DataReceipt.DataFromReceipt(l1);
            receipts.receipt.path = path;

            //אתחול המיחון של המילים הלא שימושיות
            h.initTableWordes();
            foreach (var product in receipts.products)
            {
                l2 = classificationCategory.stringToWordes(product.nameProduct);
                list += DataReceipt.anlyzeNameProduct(l2) + "  ";
            }
            if (list != null)
                dict = classificationCategory.newReceipt(list, receipts.receipt.numCompany);
            receipts.receipt.category = dict.ElementAt(0).Key;
            //
            receipt.addAllReceipt(receipts);
            return receipts;
        }
        public List<List<string>> getTextFromPath1(string text)
        {
            List<string> lines = text.Split(new[] { Environment.NewLine, "\n" },
          StringSplitOptions.None).ToList();

            List<List<string>> allWords = new List<List<string>>();

            foreach (var line in lines)
            {
                if (ocr.textToWordes(line) != null && ocr.textToWordes(line).Count > 0)
                    allWords.Add(ocr.textToWordes(line));
            }

            return allWords;
        }
       
        public  void upDateCategory(AllReceipt all, int c1)
        {
            List<string> list = new List<string>();
            List<string> l = new List<string>();
            foreach (var product in all.products)
            {
                l = classificationCategory.stringToWordes(product.nameProduct);
                list.Add(DataReceipt.anlyzeNameProduct(l));
            }
            category.updateCountWord(list,(int)all.receipt.category,c1);
          
        }

    }
}
