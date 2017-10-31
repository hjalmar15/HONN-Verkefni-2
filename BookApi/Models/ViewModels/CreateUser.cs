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

namespace BookApi.Models.ViewModels
{
    public class CreateUser
    {
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
        public string PhoneNumber { get; set; }
    }
}