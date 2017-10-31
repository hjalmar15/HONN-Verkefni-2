using System;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Models.ViewModels
{
    public class UserLoan
    {
		/// <summary>
        /// Gets or Sets LoanDate
        /// </summary>
        [Required]
        public string LoanDate { get; set; }
		/// <summary>
        /// Gets or Sets ReturnedDate
        /// </summary>
		[Required]
        public string ReturnedDate { get; set; }
    }
}