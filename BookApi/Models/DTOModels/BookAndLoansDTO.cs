using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using BookApi.Models.DTOModels;
using Newtonsoft.Json;

namespace BookApi.Models.DTOModels
{
    public class BookAndLoansDTO
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int Id { get; set; }
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
        /// <summary>
        /// Gets or Sets LoanHistory
        /// </summary>
        public IEnumerable<LoanDTO> LoanHistory {get;set;}    
    }
}