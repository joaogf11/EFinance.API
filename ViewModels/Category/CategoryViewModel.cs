using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace EFinnance.API.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Key]
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; } 

        [JsonIgnore]
        [NotMapped]
        public UserViewModel? User { get; set; }
    }
}
