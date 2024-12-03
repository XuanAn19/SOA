using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    public class OrderItemModel
    {
        [Key]
        public int id { get; set; }                
        [ForeignKey("Order")]
        public int order_id { get; set; }          
        public int product_id { get; set; }         
        public string product_name { get; set; }    
        public int quantity { get; set; }          
        [Column(TypeName = "decimal(18, 2)")]
        public decimal unit_price { get; set; }   
        [Column(TypeName = "decimal(18, 2)")]
        public decimal total_price { get; set; }

        public OrderModel Order { get; set; }
    }
}
