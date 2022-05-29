using Store_Task.Interfaces;
using Store_Task.Models;

namespace Store_Task.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext _context;
        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public List<ProductStock> GetCurrentStock()
        {
            List<ProductStock> result = new List<ProductStock>();

            var ProductIds = _context.Stocks.Select(x=>x.ProductId).Distinct().ToList();
            foreach (var item in ProductIds)
            {
                var stock = GetProductStock(item);
                var productStock = new ProductStock()
                {
                    ProductId = item,
                    Stock = stock
                };
                result.Add(productStock);
            }

            return result;
        }

        public int GetProductStock(int productId)
        {
            var allTransactions = _context.Stocks.Where(x=>x.ProductId==productId).ToList();

            var stockInTransactions = allTransactions.Where(x=>x.Type == Models.Type.StockIn).ToList();
            int stockInNo = 0;
            foreach (var item in stockInTransactions)
            {
                stockInNo += item.StockNo;
            }

            var stockOutTransactions = allTransactions.Where(x => x.Type == Models.Type.StockOut).ToList();
            int stockOutNo = 0;
            foreach (var item in stockOutTransactions)
            {
                stockOutNo += item.StockNo;
            }

            int currentStock = stockInNo - stockOutNo;
            
            return currentStock;
        }
    }
}
