﻿using EFinnance.API.ViewModels.Category;
using EFinnance.API.ViewModels.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EFinnance.API.ViewModels.Revenue
{
    public class RevenueViewModel
    {
        [Key]
        [JsonIgnore]

        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public decimal OtherIncomes { get; set; }

        [StringLength(80)]
        public string Description { get; set; }

        [Required]
        public DateTime IncomeDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Category")]
        public string CategoryId { get; set; }

        [JsonIgnore]
        public CategoryViewModel? Category { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [JsonIgnore]
        public UserViewModel? User { get; set; }

        public string FormatarData()
        {
            return IncomeDate.ToString("dd/MM/yyyy");
        }

    }
}
