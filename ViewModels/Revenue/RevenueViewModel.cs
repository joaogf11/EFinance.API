using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFinnance.API.ViewModels.Revenue
{
    public class RevenueViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public decimal OtherIncomes { get; set; }

        [StringLength(80)]
        public string Description { get; set; }

        [Required]
        public DateTime IncomeDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserViewModel User { get; set; }

        public string FormatarData()
        {
            return IncomeDate.ToString("dd/MM/yyyy");
        }

    }
}
