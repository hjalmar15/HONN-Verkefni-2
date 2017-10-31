

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BookApi.Models.DTOModels
{
    public class ReviewsDTO
    {
        
        [Required]
        public string BookName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Range(1,5, ErrorMessage="has to between 1-5")]      
        public int Stars { get; set; } 
        public string Review { get; set; }
    }
}