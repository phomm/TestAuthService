using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class CSVFileLoginRepository : ILoginRepository
    {
        const string RepositoryKeyName = "LoginRequestsFilePath";

        private IUserRepository mUserRepository;

        internal string mFilePath;

        private List<ILoginInfo> mLoginList = new List<ILoginInfo>();
        internal IEnumerable<ILoginInfo> mLogins => mLoginList;

        public CSVFileLoginRepository(IConfiguration configuration, IUserRepository userRepository)
        {
            mFilePath = configuration.GetValue<string>(RepositoryKeyName);            
            mUserRepository = userRepository;
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
            mLoginList = new List<ILoginInfo>(rows.Count());
            await Task.Factory.StartNew(() => mLoginList.AddRange(rows.Select(row => 
                new LoginInfo(row.Split(StorageCommon.ElementSeparator))))).ConfigureAwait(false);
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
        
        public string AddLoginRequest(string email)
        {
            if (mUserRepository.FindUserByEmail(email) == null)
            {
                throw new UserException($"User not found by email {email}");
            }
            mLoginList.RemoveAll(login => login.RegistrationEmail == email);
            var result = new LoginInfo(email);
            mLoginList.Add(result);
            // temporarily as draft, should be some sort of scheduled task running on idle
            _ = UpdateStorage().ConfigureAwait(false);
            return result.Token;
        }
        
        public IEnumerable<ILoginInfo> GetLoginRequests()
        {
            return mLogins;
        }

        private IEnumerable<string> DumpStorage()
        {
            return mLoginList.Select(user => user.ToStorageString());
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

        public LoginRequestResult ValidateLoginRequest(string token)
        {
            return mLogins.FirstOrDefault(info => info.Token == token) switch
            {
                null => LoginRequestResult.RequestNotFound,
                ILoginInfo li when li.TokenLife < DateTime.Now => LoginRequestResult.RequestIsExpired,
                ILoginInfo li when mUserRepository.FindUserByEmail(li.RegistrationEmail) == null => LoginRequestResult.UserNotFound,
                { } => LoginRequestResult.Success
            };
        }
    }
}
