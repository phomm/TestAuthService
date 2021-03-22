using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class CSVFileUserRepository : IUserRepository
    {
        const string RepositoryKeyName = "UserRepositoryFilePath"; 

        internal string mFilePath;

        private List<IUserInfo> mUserList = new List<IUserInfo>();
        internal IEnumerable<IUserInfo> mUsers => mUserList;

        public CSVFileUserRepository(IConfiguration configuration)
        {            
            mFilePath = configuration.GetValue<string>(RepositoryKeyName);
            _ = InitRepository().ConfigureAwait(false);
        }

        private void PrepareStorage()
        {
            try
            {
                using (File.OpenWrite(mFilePath)) { }
            }
            catch (IOException ex)
            {
                throw new StorageException($"File {mFilePath} is unavailable", ex);
            }
        }

        private async Task<bool> ParseStorage(IEnumerable<string> rows)
        {
            mUserList = new List<IUserInfo>(rows.Count());
            await Task.Factory.StartNew(() => mUserList.AddRange(rows.Select(row => 
                new UserInfo(row.Split(StorageCommon.ElementSeparator))))).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> InitRepository()
        {
            try
            {
                PrepareStorage();
                var lines = await File.ReadAllLinesAsync(mFilePath).ConfigureAwait(false);
                return await ParseStorage(lines);
            }
            catch (Exception ex)
            {
                throw new StorageException($"File {mFilePath} read", ex);
            }
        }

        private IEnumerable<string> DumpStorage()
        {
            return mUserList.Select(user => user.ToStorageString());            
        }

        public async Task<bool> UpdateStorage()
        {
            try
            {
                PrepareStorage();
                var lines = DumpStorage();
                await File.WriteAllLinesAsync(mFilePath, lines);
                return true;
            }
            catch (Exception ex)
            {
                throw new StorageException($"File {mFilePath} read", ex);
            }
        }

        public bool AddUser(IUserInfo userInfo)
        {
            var user = FindUserByEmail(userInfo.RegistrationEmail);
            if (user != null)
            {
                throw new UserException($"User with email {user.RegistrationEmail} already exists ");
            }
            mUserList.Add(userInfo);
            // temporarily as draft, should be some sort of scheduled task running on idle
            _ = UpdateStorage().ConfigureAwait(false);
            return true;
        }

        public IUserInfo FindUserByEmail(string email)
        {
            return FindUserByPredicate(user => user.RegistrationEmail == email);
        }

        public IUserInfo FindUserByPredicate(Func<IUserInfo, bool> predicate)
        {
            return mUsers.FirstOrDefault(predicate);
        }

        public IEnumerable<IUserInfo> GetUsers()
        {
            return mUsers;
        }

        public IEnumerable<IUserInfo> GetUsersByPredicate(Func<IUserInfo, bool> predicate)
        {
            return mUsers.Where(predicate);
        }
    }
}
