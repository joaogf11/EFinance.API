using EFinnance.API.ViewModels.Expense;
using EFinnance.API.ViewModels.Revenue;
using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EFinnance.API.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Key]
        [JsonIgnore] 
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        
        [JsonIgnore] 
        public UserViewModel User { get; set; }

    }
}
