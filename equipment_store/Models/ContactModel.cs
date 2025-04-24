using equipment_store.Repository.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace equipment_store.Models
{
    public class ContactModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tiêu đề website không được để trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập vị trí")]
        public string Map { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin liên hệ")]
        public string Description { get; set; }

        public string LogoImg { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
