using System.ComponentModel.DataAnnotations;

namespace Product.API.Models
{
    public class ProductModel
    {
        [Key]
        public int id {  get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public DateTime created_at { get; set;}
        public DateTime updated_at { get; set;}
    }
}
