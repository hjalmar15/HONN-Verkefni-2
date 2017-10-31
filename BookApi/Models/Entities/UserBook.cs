using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BookApi.Models.Entities
{
	/// <summary>
    /// A UserBook entity class
    /// </summary>
    public class UserBook
    {
		/// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int Id { get; set; }
		/// <summary>
        /// Gets or Sets UserId
        /// </summary>
        public int UserId { get; set; }
		/// <summary>
        /// Gets or Sets BookId
        /// </summary>
        public int BookId { get; set; }
		/// <summary>
        /// Gets or Sets LoanDate
        /// </summary>
        public DateTime LoanDate { get; set; }
		/// <summary>
        /// Gets or Sets ReturnedDate
        /// </summary>
        public DateTime ReturnedDate { get; set; }
    }
}