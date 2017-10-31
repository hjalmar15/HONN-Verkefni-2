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
    public class UserBook
    {
        public int Id { get; set; }
        [DataMember(Name="UserId")]
        public int UserId { get; set; }

        [DataMember(Name="BookId")]
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnedDate { get; set; }
    }
}