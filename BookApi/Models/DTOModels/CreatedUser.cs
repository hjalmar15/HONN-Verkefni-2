using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using BookApi.Models.Entities;

namespace BookApi.Models.DTOModels
{
    public class CreatedUser
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public int? Id { get; set; }
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