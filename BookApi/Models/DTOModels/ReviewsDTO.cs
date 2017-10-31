

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BookApi.Models.DTOModels
{
    public class ReviewsDTO
    {
        /// <summary>
        /// Gets or Sets BookName
        /// </summary>
        [Required]
        public string BookName { get; set; }
		/// <summary>
        /// Gets or Sets UserName
        /// </summary>
        [Required]
        public string UserName { get; set; }
		/// <summary>
        /// Gets or Sets Stars
        /// </summary>
        [Range(1,5, ErrorMessage="has to between 1-5")]      
        public int Stars { get; set; }
		/// <summary>
        /// Gets or Sets Review
        /// </summary>
        public string Review { get; set; }
    }
}