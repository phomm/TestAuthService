using System;

namespace AuthenticationService.Models
{
    public interface IUserInfo: IInfo
    {
        public string ProfileName { get; set; }
        public string RegistrationEmail { get; set; }
        public string Password { get; set; }

    }
}