using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Handles;
using UserService.Infrastructure.GenericRepository;
using UserService.Infrastructure.SpecificRepository;
using UserService.Mappings.v2;
using UserService.Models.v2;

namespace UserService.Module.v2
{
    public interface IUserModule
    {
        public Task<User_Info_Response> GetUserInfoFromEmailAsync(string email);
    }

    public class UserModule : IUserModule
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IUserAuthorizeRepository _IUserAuthorizeRepository;

        public UserModule(IUserRepository IUserRepository, IUserAuthorizeRepository IUserAuthorizeRepository)
        {
            _IUserAuthorizeRepository = IUserAuthorizeRepository;
            _IUserRepository = IUserRepository;
        }
        public async Task<User_Info_Response> GetUserInfoFromEmailAsync(string email)
        {
            var _db_user = await _IUserRepository.GetUserByEmailAsync(email);
            if (_db_user == null)
                throw new AppCustomException_v2("user not found", false);
            else
            {
                var _db_roles = await _IUserAuthorizeRepository.GetTotalRolesOfUserAsync(_db_user);
                return _db_user.ToAppModel(_db_roles);
            }
        }
    }
}
