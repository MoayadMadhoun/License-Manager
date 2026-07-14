using System.ComponentModel.DataAnnotations;

namespace LicenseManagerMinimalAPI.Models
{
    public class AppLicence
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string UserNmae { get; set; }
        [StringLength(20), MinLength(10)]
        [Required]
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
