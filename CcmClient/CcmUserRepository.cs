using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CcmClient
{
    public class CcmUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Password { get; set; }
    }




    public class CcmUserRepository :
        IUserPasswordStore<CcmUser>,
        IQueryableUserStore<CcmUser>
    {
        private static Dictionary<long, CcmUser> _userTable = new Dictionary<long, CcmUser>();

        public IQueryable<CcmUser> Users => _userTable.Values.AsQueryable();

        public Task<IdentityResult> CreateAsync(CcmUser user, CancellationToken cancellationToken)
        {
            user.Id = _userTable.Keys.Max() + 1;

            _userTable.Add(user.Id, user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(CcmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose() { }

        public Task<CcmUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (_userTable.TryGetValue(long.Parse(userId), out CcmUser user))
                return Task.FromResult(user);

            //var user = new CcmUser { Id = int.Parse(userId), FirstName = "MArtin", LastName = "Holly" };
            return Task.FromResult(user);
        }

        public Task<CcmUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = _userTable.Values
                .FirstOrDefault(x => x.NormalizedUserName == normalizedUserName);

            //TODO : get this from user name / email
            //var user = new CcmUser { Id = 2, FirstName = "MArtin", LastName = "Holly" };

            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(CcmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(CcmUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Password);
        }

        public Task<string> GetUserIdAsync(CcmUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(CcmUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(CcmUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public Task SetNormalizedUserNameAsync(CcmUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(CcmUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }


        public Task SetUserNameAsync(CcmUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(CcmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
