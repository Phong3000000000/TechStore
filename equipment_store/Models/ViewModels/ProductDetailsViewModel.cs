using System.ComponentModel.DataAnnotations;

namespace equipment_store.Models.ViewModels
{
	public class ProductDetailsViewModel
	{
		public ProductModel ProductDetails { get; set; }


		[Required(ErrorMessage = "Yêu cầu nhập đánh giá sản phẩm")]
		public string Comment { get; set; }

		[Required(ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }
	
		[Required(ErrorMessage = "Yêu cầu nhập email")]
		public string Email { get; set; }
	}
}
