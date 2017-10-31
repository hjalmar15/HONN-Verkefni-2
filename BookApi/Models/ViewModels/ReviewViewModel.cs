
using System.ComponentModel.DataAnnotations;

namespace BookApi.Models.ViewModels
{
    public class ReviewViewModel
    {
		/// <summary>
        /// Gets or Sets Stars
        /// </summary>
        [Required]
        public int Stars { get; set; }
		/// <summary>
        /// Gets or Sets UserReview
        /// </summary>
        [Required]
        public string UserReview { get; set; }
    }
}