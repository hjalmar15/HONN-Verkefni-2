using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BookApi.Models.ViewModels
{
    public class NewBook
    {
                /// <summary>
        /// Initializes a new instance of the <see cref="Book" /> class.
        /// </summary>
        /// <param name="Title">Title.</param>
        /// <param name="Author">Author.</param>
        /// <param name="DatePublished">DatePublished.</param>
        /// <param name="ISBN">ISBN.</param>
        
        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [DataMember(Name="title")]
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Gets or Sets Author
        /// </summary>
        [DataMember(Name="author")]
        [Required]
        public string Author { get; set; }
        /// <summary>
        /// Gets or Sets DatePublished
        /// </summary>
        [DataMember(Name="datePublished")]
        [Required]
        public string DatePublished { get; set; }
        /// <summary>
        /// Gets or Sets ISBN
        /// </summary>
        [DataMember(Name="ISBN")]
        [Required]
        public string ISBN { get; set; }
    }
}