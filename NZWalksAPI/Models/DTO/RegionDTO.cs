using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class RegionDTO
    {
        //public Guid Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Min Length: 3")]
        [MaxLength(3, ErrorMessage = "Max Length: 3")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Max Length: 100")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
