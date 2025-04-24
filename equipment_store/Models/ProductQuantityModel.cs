using System.ComponentModel.DataAnnotations;

namespace equipment_store.Models
{
	public class ProductQuantityModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Số lượng sản phẩm không được để trống")]
		public int Quantity { get; set; }

		public int ProductId { get; set; }
	
		public DateTime DateCreated { get; set; }
		
	}
}
