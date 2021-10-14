using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.v1
{
    public class Login_Request
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class User_Info_Response
    {
        public string username { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class User_Info_Request : User_Info_Response
    {
   
    }
}
