using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.v2
{
    public class Login_Request
    {
        public string email { get; set; }
        public string first_name { get; set; }
    }

    public class User_Info_Response
    {
        public string username { get; set; }
        public string email { get; set; }
        public string[] role { get; set; }
        public string tel { get; set; }
    }
}
