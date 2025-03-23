using EFinnance.API.ViewModels.Expense;
using EFinnance.API.ViewModels.Revenue;
using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFinnance.API.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public UserViewModel User { get; set; }

        public ICollection<RevenueViewModel> Revenues { get; set; }
        public ICollection<ExpenseViewModel> Expenses { get; set; }
    }
}
