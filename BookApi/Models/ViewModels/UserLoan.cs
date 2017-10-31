using System;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Models.ViewModels
{
    public class UserLoan
    {
        [Required]
        public string LoanDate { get; set; }
        public string ReturnedDate { get; set; }
    }
}