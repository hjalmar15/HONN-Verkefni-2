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
    /// A book entity class
    /// </summary>
    public class Book
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
    }
}
