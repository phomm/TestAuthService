using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public interface ILoginRepository: IRepository
    {
        IEnumerable<ILoginInfo> GetLoginRequests();
        string AddLoginRequest(string email);
        LoginRequestResult ValidateLoginRequest(string token);
    }
}
