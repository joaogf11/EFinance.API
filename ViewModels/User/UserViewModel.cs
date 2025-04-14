using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.Expense;
using EFinnance.API.ViewModels.Revenue;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFinnance.API.ViewModels.User
{
    public class UserViewModel : IdentityUser
    {
        public ICollection<CategoryViewModel>? Categories { get; set; }
        public ICollection<RevenueViewModel>? Revenues { get; set; }
        public ICollection<ExpenseViewModel>? Expenses { get; set; }
    }
}
