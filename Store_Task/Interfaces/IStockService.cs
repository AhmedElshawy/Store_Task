using Store_Task.Models;

namespace Store_Task.Interfaces
{
    public interface IStockService
    {
        List<ProductStock> GetCurrentStock();
        int GetProductStock(int productId);
    }
}
