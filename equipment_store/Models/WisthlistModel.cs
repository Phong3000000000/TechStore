using System.ComponentModel.DataAnnotations;

namespace equipment_store.Models
{
    public class WisthlistModel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public ProductModel Product { get; set; }
    }
}
