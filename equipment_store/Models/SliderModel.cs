using equipment_store.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace equipment_store.Models
{
    public class SliderModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên slider không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Mô tả cho slider không được để trống")]
        public string Description { get; set; }
        public string? Status { get; set; }

        public string Imgae { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
