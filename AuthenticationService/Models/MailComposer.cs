using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class MailComposer
    {
        const string changePasswordStart = @"<p>Dear player,
In order to log in to your account so you can change your password click the
following link:</p>";
        const string changePasswordEnd = @"<p>Note that this link is only valid for 1 hour, after which it expires!  


Happy playing! 
Greentube Support Team</p>";

        const string defaultChangePasswordUrlCaption = "Change Password";

        const string br = "<br>";

        public static string ComposeChangePasswordLetter(string href, string caption = defaultChangePasswordUrlCaption)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(br, changePasswordStart.Split(Environment.NewLine));
            sb.Append($"<a href=\"{ href}\">{caption}</a>");
            sb.Append(br);
            sb.AppendJoin(br, changePasswordEnd.Split(Environment.NewLine));

            return sb.ToString(); 
        }
    }
}
