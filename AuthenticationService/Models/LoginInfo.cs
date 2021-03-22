using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class LoginInfo: ILoginInfo
    {
        public const int TokenLifeHours = 1;

        public string RegistrationEmail { get; set; }
        
        public string Token { get; set; }

        public DateTime TokenLife { get; set; }

        public LoginInfo(string registrationEmail)
            : this(registrationEmail, Guid.NewGuid().ToString()) { }

        public LoginInfo(string registrationEmail, string token)
        {
            RegistrationEmail = registrationEmail;
            Token = token;
            TokenLife = DateTime.Now.AddHours(TokenLifeHours);
        }

        public LoginInfo(IEnumerable<string> elements)
        {
            var propsCount = this.GetType().GetProperties().Length;
            var elementsCount = elements.Count();
            if (elementsCount != propsCount)
            {
                throw new UserException($"Wrong elements for creating login request, {elementsCount} of {propsCount} provided");
            }

            RegistrationEmail = elements.ElementAt(0);
            Token = elements.ElementAt(1);
            TokenLife = DateTime.Parse(elements.ElementAt(2));
        }

        public string ToStorageString()
        {
            var elements = new[] { RegistrationEmail, Token, TokenLife.ToString()};
            return string.Join(StorageCommon.ElementSeparator, elements);
        }
    }
}
