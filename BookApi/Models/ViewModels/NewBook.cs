using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BookApi.Models.ViewModels
{
    public class NewBook
    {
        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Gets or Sets Author
        /// </summary>
        [Required]
        public string Author { get; set; }
        /// <summary>
        /// Gets or Sets DatePublished
        /// </summary>
        [Required]
        public string DatePublished { get; set; }
        /// <summary>
        /// Gets or Sets ISBN
        /// </summary>
        [Required]
        public string ISBN { get; set; }
    }
}