using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public interface ILoginInfo: IInfo
    {
        string RegistrationEmail { get; set; }

        string Token { get; set; }

        DateTime TokenLife { get; set; }        
    }
}
