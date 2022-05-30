using Bll.Algorithm;
using Dal;
using Dto.DataToObject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Db
{
    public class Receipts
    {
        Category category;
        public int addAllReceipt(AllReceipt all)
        {
            all.receipt.myUser = Global.UserID;
            
            int idReceipt = AddReceipt(all.receipt);
            foreach (var product in all.products)
                product.receipt = idReceipt;
            AddProdactes(all.products);
            string path = null;
            if (all.receipt.path != null)
            {
                path = savePath(all.receipt.path, idReceipt);
                upDatePath(idReceipt, path);
            }
               
            return idReceipt;
        }
        public void upDatePath(int id,string path)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                Receipt_tbl r = db.Receipt_tbl.FirstOrDefault(a => a.Id == id);
                r.path = path;
                db.SaveChanges();
            }

        }
        public string savePath(string path,int id)
        {
            string p = path;
            int index = path.IndexOf("App_Data\\");
            path=path.Remove(index);
            string originalFileName = String.Concat(path,(id.ToString()).Trim(new Char[] { '"' }));
            if (File.Exists(originalFileName))
            {
                File.Delete(originalFileName);
            }
            File.Move(p, originalFileName);
            return originalFileName;
        }

        public List<AllReceipt> getAllReceipt()
        {
            ReceiptDto receipt = new ReceiptDto();
            List<AllReceipt> lstr = new List<AllReceipt>();
            List<ProductsDto> lstp = new List<ProductsDto>();
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var item in db.Receipt_tbl)
                {
                    if (item.myUser == "2")
                    {
                        receipt = getReceipt(item.Id);
                        lstp = getProdactesDto(receipt.Id);
                        lstr.Add(new AllReceipt(receipt, lstp));
                    }
                }
            }
            return lstr;
        }
        public List<AllReceipt> getAllReceiptPerCategory(string category)
        {
            ReceiptDto receipt = new ReceiptDto();
            List<AllReceipt> lstr = new List<AllReceipt>();
            List<ProductsDto> lstp = new List<ProductsDto>();
            List<AllReceipt> lst = getAllReceipt();
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                int categoryNum = db.Category_tbl.Where(x => x.nameCategory == category).Select(y => y.Id).First();
                foreach (var item in lst)
                {
                    if (item.receipt.category == categoryNum)
                    {
                        receipt = getReceipt(item.receipt.Id);
                        lstp = getProdactesDto(receipt.Id);
                        lstr.Add(new AllReceipt(receipt, lstp));
                    }
                }
            }
            return lstr;
        }
        public Dictionary<string, decimal> getAllCategorySum()
        {
            Dictionary<string, decimal> sumPerCategory = new Dictionary<string, decimal>();
            decimal sum = 0;
            List<CategoryDto> lc = new List<CategoryDto>();
            List<AllReceipt> lstr = new List<AllReceipt>();
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var item1 in db.Category_tbl)
                {
                    lstr = getAllReceiptPerCategory(item1.nameCategory);
                    foreach (var item2 in lstr)
                    {
                        sum += (decimal)item2.receipt.totalSum;
                    }
                    sumPerCategory.Add(item1.nameCategory, sum);
                    sum = 0;
                }
            }
            return sumPerCategory;
        }
        public void upDateReceipt(AllReceipt all,int category)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                StartAlgoritem s=new StartAlgoritem();  
                Receipt_tbl r = db.Receipt_tbl.FirstOrDefault(a => a.Id == all.receipt.Id);
                r.dateReceipt = all.receipt.dateReceipt;
                r.numCompany = all.receipt.numCompany;
                r.nameShop = all.receipt.nameShop;
                r.totalSum = all.receipt.totalSum;
                List<Products_tbl> productsDtos = getProdactes(all.receipt.Id);
                Products_tbl p = new Products_tbl();
                foreach (Products_tbl product in productsDtos)
                {
                    p = db.Products_tbl.FirstOrDefault(a => a.Id == product.Id);
                    p.sumProduct = product.sumProduct;
                    p.nameProduct = product.nameProduct;
                    p.amount = product.amount;
                    db.SaveChanges();
                }
                if (all.receipt.category != category)
                    s.upDateCategory(all,category);
                db.SaveChanges();
            }
        }
      
        public void deleteReceipt(int idReceipt)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var product in db.Products_tbl)
                {
                    if (product.receipt == idReceipt)
                    {
                        db.Products_tbl.Remove(db.Products_tbl.First(a => a.Id == product.Id));
                        db.SaveChanges();
                    }
                }
                db.Receipt_tbl.Remove(db.Receipt_tbl.First(a => a.Id == idReceipt));
                db.SaveChanges();
            }
        }
        //receipt
        public ReceiptDto getReceipt(int id)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                return ReceiptDto.DalToDto(db.Receipt_tbl.First(a => a.Id == id));

            }
        }
        public int AddReceipt(ReceiptDto r)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                Receipt_tbl receipt = db.Receipt_tbl.Add(r.DtoToDal());
                db.SaveChanges();
                return receipt.Id;
            }
        }
        //product
        public List<Products_tbl> getProdactes(int receiptId)
        {
            List<Products_tbl> lst = new List<Products_tbl>();
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var item in db.Products_tbl)
                {
                    if (item.receipt == receiptId)
                        lst.Add(item);
                }
                return lst;
            }
        }
        public List<ProductsDto> getProdactesDto(int receiptId)
        {
            List<ProductsDto> lst = new List<ProductsDto>();
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var item in db.Products_tbl)
                {
                    if (item.receipt == receiptId)
                        lst.Add(ProductsDto.DalToDto( item));
                }
                return lst;
            }
        }



        public void AddProdactes(List<ProductsDto> products)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var p in products)
                    db.Products_tbl.Add(p.DtoToDal());
                db.SaveChanges();
            }
        }
        public ProductsDto GetProduct(int id)
        {
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                return ProductsDto.DalToDto(db.Products_tbl.First(a => a.Id == id));
            }
        }
        public List<string> getUnusingWord()
        {
            List<string> list = new List<string>();
            using (Receipts_dbEntities1 db = new Receipts_dbEntities1())
            {
                foreach (var item in db.UnusingWord_tbl)
                {
                    UnusingWordDto un = UnusingWordDto.DalToDto(item);
                    list.Add(un.unusingWord);

                }
            }
            return list;
        }
    }
}
