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
        [DataMember(Name="id")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [DataMember(Name="title")]
        public string Title { get; set; }
        /// <summary>
        /// Gets or Sets Author
        /// </summary>
        [DataMember(Name="author")]
        public string Author { get; set; }
        /// <summary>
        /// Gets or Sets DatePublished
        /// </summary>
        [DataMember(Name="datePublished")]
        public string DatePublished { get; set; }
        /// <summary>
        /// Gets or Sets ISBN
        /// </summary>
        [DataMember(Name="ISBN")]
        public string ISBN { get; set; }
        /// <summary>
        /// Gets or Sets LoanHistory
        /// </summary>
        [DataMember(Name="LoanHistory")]
        public IEnumerable<LoanDTO> LoanHistory {get;set;}    
    }
}