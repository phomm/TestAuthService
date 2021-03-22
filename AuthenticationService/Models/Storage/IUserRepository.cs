using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public interface IUserRepository: IRepository
    {
        IEnumerable<IUserInfo> GetUsers();
        IUserInfo FindUserByEmail(string email);
        bool AddUser(IUserInfo userInfo);

        IUserInfo FindUserByPredicate(Func<IUserInfo, bool> predicate);
        IEnumerable<IUserInfo> GetUsersByPredicate(Func<IUserInfo, bool> predicate);
    }
}
