using equipment_store.Repository.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace equipment_store.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Tên sản phẩm không được để trống")]
		public string Name { get; set; }

		public string slug { get; set; }
		[Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
		public string Description { get; set; }
        [Required(ErrorMessage = "Hãy nhập giá cho sản phẩm")]
		[Range(0.01, double.MaxValue)]
		[Column(TypeName ="decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required, Range(1, int.MaxValue,ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int BrandId {  get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Image { get; set; } 

		public RatingModel Rating { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload { get; set; }


	}
}
