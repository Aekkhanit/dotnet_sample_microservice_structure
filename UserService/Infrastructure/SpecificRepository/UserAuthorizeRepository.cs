using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Infrastructure.Context_Models;

namespace UserService.Infrastructure.SpecificRepository
{
    public interface IUserAuthorizeRepository
    {
        public Task<string[]> GetTotalRolesOfUserAsync(TB_User user);
    }
    public class UserAuthorizeRepository : IUserAuthorizeRepository
    {
        private readonly MyDB_Context _MyDB_Context;
        public UserAuthorizeRepository(MyDB_Context MyDB_Context)
        {
            _MyDB_Context = MyDB_Context;
        }

        public async Task<string[]> GetTotalRolesOfUserAsync(TB_User user)
        {
            //you can code here to connect your context with many table for relation of data or some customize logic
            //eg. tb_user, tb_role, tb_permission

            //retreive role from db (eg. var _db_roles=_MyDB_Context.TB_Roles.ToList()
            switch (user.Type)
            {
                case "admin":
                    return await Task.FromResult(new string[] { "admin", "super_admin" });

                default:
                    return await Task.FromResult(new string[] { "staff" });
            }

        }
    }
}
