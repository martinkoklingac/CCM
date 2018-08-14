using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CcmClient
{
    public class CcmUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CcmUserRepository :
        IUserStore<CcmUser>
    {
        public Task<IdentityResult> CreateAsync(CcmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(CcmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<CcmUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = new CcmUser { Id = int.Parse(userId), FirstName = "MArtin", LastName = "Holly" };
            return Task.FromResult(user);
        }

        public Task<CcmUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            //TODO : get this from user name / email


            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(CcmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(CcmUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(CcmUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.FirstName);
        }

        public Task SetNormalizedUserNameAsync(CcmUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
