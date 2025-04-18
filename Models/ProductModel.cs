﻿using System.ComponentModel.DataAnnotations;
namespace equipment_store.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Tên sản phẩm phải ít nhất 4 ký tự")]
		public string Name { get; set; }

		public string slug { get; set; }
		[Required, MinLength(4, ErrorMessage = "Mô tả sản phẩm phải ít nhất 4 ký tự")]
		public string Description { get; set; }
		[Required, MinLength(1, ErrorMessage = "Hãy nhập giá cho sản phẩm")]
		public decimal Price { get; set; }
		public int BrandId {  get; set; }
		public int CategoryId { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Image { get; set; }

	}
}
