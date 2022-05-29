using System.ComponentModel.DataAnnotations.Schema;

namespace Store_Task.Models
{
    [Table("Product")]
    public class Product
    {       
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        
    }
}
