using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Task.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }

        [Column("Stock")]
        public int StockNo { get; set; }
        public Type Type { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
