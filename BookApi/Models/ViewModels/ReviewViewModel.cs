
using System.ComponentModel.DataAnnotations;

namespace BookApi.Models.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        public int Stars { get; set; }
        [Required]
        public string UserReview { get; set; }
    }
}