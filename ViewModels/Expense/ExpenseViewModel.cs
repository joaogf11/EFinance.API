using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EFinnance.API.ViewModels.Expense
{
    public class ExpenseViewModel
    {
        [Key]
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public string CategoryId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public CategoryViewModel? Category { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public UserViewModel? User { get; set; }

        public string FormatarData()
        {
            return DueDate.ToString("dd/MM/yyyy");
        }

    }
}
