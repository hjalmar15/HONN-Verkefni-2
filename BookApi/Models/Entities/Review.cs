using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BookApi.Models.Entities
{
    /// <summary>
    /// A review entity class
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Gets or Sets ReviewID
        /// </summary>
        public int ReviewID { get; set; }
        /// <summary>
        /// Gets or Sets BookId
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or Sets Stars
        /// </summary>
        public int Stars { get; set; }
        /// <summary>
        /// Gets or Sets UserReview
        /// </summary>
        public string UserReview { get; set; }
    }
}
