using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Infrastructure.Context_Models;
using UserService.Models.v2;

namespace UserService.Mappings.v2
{
    public static class FromContext
    {
        public static User_Info_Response ToAppModel(this TB_User source, string[] roles)
        {
            return new User_Info_Response()
            {
                email = source.Email,
                role = roles,
                tel = source.Tel,
                username = source.Username
            };
        }
    }

 
}
