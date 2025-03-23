using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFinnance.API.ViewModels.Expense
{
    public class ExpenseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserViewModel User { get; set; }

        public string FormatarData()
        {
            return DueDate.ToString("dd/MM/yyyy");
        }

    }
}
