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

        public ICollection<Revenue> Revenues { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
