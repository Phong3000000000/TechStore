using System.ComponentModel.DataAnnotations;
namespace equipment_store.Models
{
	public class BrandModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Tên thương hiệu phải ít nhất 4 ký tự")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Mô tả thương hiệu phải ít nhất 4 ký tự")]
		public string Description { get; set; }
		[Required]
		public string slug { get; set; }
		public string Status { get; set; }

	}
}
