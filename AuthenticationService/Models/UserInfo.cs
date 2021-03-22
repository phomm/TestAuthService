using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    [Serializable]
    public class UserInfo: IUserInfo
    {
        public string ProfileName { get; set; }
        public string RegistrationEmail { get; set; }
        public string Password { get; set; }

        public UserInfo()
        {

        }

        public UserInfo(string profileName, string registrationEmail, string password)
        {
            ProfileName = profileName;
            RegistrationEmail = registrationEmail;
            Password = password;
        }

        public UserInfo(IEnumerable<string> elements)
        {
            var propsCount = this.GetType().GetProperties().Length;
            var elementsCount = elements.Count();
            if (elementsCount != propsCount)
            {
                throw new UserException($"Wrong elements for creating user, {elementsCount} of {propsCount} provided");
            }

            ProfileName = elements.ElementAt(0);
            RegistrationEmail = elements.ElementAt(1);
            Password = elements.ElementAt(2);
        }

        public string ToStorageString()
        {
            var elements = new[] { ProfileName, RegistrationEmail, Password };
            return string.Join(StorageCommon.ElementSeparator, elements);
        }
    }
}
