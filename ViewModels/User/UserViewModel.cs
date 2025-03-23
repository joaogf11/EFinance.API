using EFinnance.API.ViewModels.Category;
using System.ComponentModel.DataAnnotations;

namespace EFinnance.API.ViewModels.User
{
    public class UserViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; }
        public ICollection<RevenueViewModel> Revenues { get; set; }
        public ICollection<ExpenseViewModel> Expenses { get; set; }
    }
}
