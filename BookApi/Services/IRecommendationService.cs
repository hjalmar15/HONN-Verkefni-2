using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;

namespace BookApi.Services
{
    public interface IRecommendationService
    {
        IEnumerable<BookDTO> getRecommendations(int user_id);
    }
}