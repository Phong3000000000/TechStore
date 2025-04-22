using System.ComponentModel.DataAnnotations;

namespace equipment_store.Models
{
	public class RatingModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }
		[Required(ErrorMessage ="Yêu cầu nhập đánh giá sản phẩm")]
		public string Comment { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập email")]
		public string Email { get; set; }
		public string Star { get; set; }
		public int ProductId { get; set; }

		public ProductModel Product { get; set; }
	}
}
