using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public interface IRepository
    {
        Task<bool> InitRepository();
        Task<bool> UpdateStorage();
    }
}
