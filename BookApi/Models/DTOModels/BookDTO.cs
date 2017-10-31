using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BookApi.Models.DTOModels
{
    public class BookDTO
    {

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or Sets Author
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Gets or Sets DatePublished
        /// </summary>
        public string DatePublished { get; set; }
        /// <summary>
        /// Gets or Sets ISBN
        /// </summary>
        public string ISBN { get; set; }
    }
}