using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public enum LoginRequestResult
    {
        Success,
        RequestNotFound,
        RequestIsExpired,
        UserNotFound
    }
}
