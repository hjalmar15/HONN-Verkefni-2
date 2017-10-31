using System;

namespace BookApi.Models.DTOModels
{
    public class LoanDTO
    {
        public CreatedUser Loanee { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime ReturnedDate { get; set; }
    }
}