using System.ComponentModel.DataAnnotations;

namespace equipment_store.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Hãy nhập UserName")]
		public string Username { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Hãy nhập Password")]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }

	}
}
