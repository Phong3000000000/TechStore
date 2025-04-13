using System.ComponentModel.DataAnnotations;
namespace equipment_store.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage ="Danh mục phải ít nhất 4 ký tự")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage ="Mô tả danh mục phải ít nhất 4 ký tự")]
		public string Description { get; set; }
		[Required]
		public string slug { get; set; }
		public string Status { get; set; }

	}
}
