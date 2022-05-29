using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Store_Task.Interfaces;
using Store_Task.Models;

namespace Store_Task.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IStockService _stockService;
        public ProductController(AppDbContext context , IStockService stockService)
        {
            _context = context;
            _stockService = stockService;
        } 
        
        public IActionResult Index(string name , int? min, int? max , bool hasStock)
        {
            var products = _context.Products.AsQueryable();
            if(name != null)
                products = products.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            if(min != null && max !=null)
                products = products.Where(x => x.Price >= min && x.Price <= max);
            if (hasStock)
            {             
                var productsHasStock = _stockService.GetCurrentStock().Where(x=>x.Stock> 0)
                    .Select(s=>s.ProductId).ToList();
                
                products = products.Where(x => productsHasStock.Contains(x.Id));              
            }

            TempData["data"] = JsonConvert.SerializeObject(products.ToList());

            return View(products.ToList());
        }

        public IActionResult DownloadExcleFile()
        {
            var FilteredProducts = JsonConvert.DeserializeObject<List<Product>>(TempData["data"].ToString());           

            // to get rid of the LicenseException
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // creating object of type ExcelPackage
            ExcelPackage package = new ExcelPackage();

            // adding new excle worksheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Excel Report");

            //writing columns names in the worksheet
            worksheet.Cells["A1"].Value = "Product Name";
            worksheet.Cells["B1"].Value = "Price";
            worksheet.Cells["C1"].Value = "Current Stock";

            // specifiying the starting row
            int rowToStart = 2;

            //seeding data into the sheet
            foreach (var item in FilteredProducts)
            {
                worksheet.Cells[string.Format("A{0}", rowToStart)].Value = item.Name;
                worksheet.Cells[string.Format("B{0}", rowToStart)].Value = item.Price;              
                worksheet.Cells[string.Format("C{0}", rowToStart)].Value = _stockService.GetProductStock(item.Id);
                rowToStart++;
            }

            //adjusting colums to fit the data
            worksheet.Cells["A:AZ"].AutoFitColumns();

            // gets the file contents as byte array
            var fileContents = package.GetAsByteArray();

            return File(fileContents, "application/ms-excel", "Report.xlsx");
        }
                  
    }
}
