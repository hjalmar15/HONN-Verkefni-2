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
    /// A user entity class
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or Sets PhoneNumber
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
