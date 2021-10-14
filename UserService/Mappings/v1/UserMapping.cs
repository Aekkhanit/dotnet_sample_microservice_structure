using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Infrastructure.Context_Models;
using UserService.Models.v1;

namespace UserService.Mappings.v1
{
    public static class FromContext
    {
        public static User_Info_Response ToAppModel(this TB_User source)
        {
            return new User_Info_Response()
            {
                email = source.Email,
                first_name = source.First_Name,
                last_name = source.Last_Name,
                username = source.Username
            };
        }
    }

    public static class FromApp
    {
        public static TB_User ToContextModel(this User_Info_Request source)
        {
            return new TB_User()
            {
                Email = source.email,
                First_Name = source.first_name,
                Last_Name = source.last_name,
                Username = source.username
            };
        }
    }
}
