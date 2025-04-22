using System.ComponentModel.DataAnnotations;
namespace equipment_store.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(3, ErrorMessage ="Danh mục phải ít nhất 3 ký tự")]
		public string Name { get; set; }
		[Required, MinLength(3, ErrorMessage ="Mô tả danh mục phải ít nhất 3 ký tự")]
		public string Description { get; set; }
		[Required]
		public string slug { get; set; }
		public string Status { get; set; }

	}
}
