using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Handles;
using UserService.Infrastructure.GenericRepository;
using UserService.Mappings.v1;
using UserService.Models.v1;

namespace UserService.Module.v1
{
    public interface IUserModule
    {
        public Task<User_Info_Response> GetUserInfoByUsernameAsync(string username);
    }

    public class UserModule : IUserModule
    {
        private readonly IUserRepository _IUserRepository;
        public UserModule(IUserRepository IUserRepository)
        {
            _IUserRepository = IUserRepository;
        }

        public async Task<User_Info_Response> GetUserInfoByUsernameAsync(string username)
        {
            var _db_user = await _IUserRepository.GetUserByUsernameAsync(username);
            return _db_user == null ? throw new AppCustomException_v1("user not found") : _db_user.ToAppModel();
        }
    }
}
