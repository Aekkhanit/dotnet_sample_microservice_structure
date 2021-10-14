using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Handles
{
    public class AppCustomException_v1 : Exception
    {
        public string api_version => "1";
        public AppCustomException_v1() : base() { }

        public AppCustomException_v1(string message) : base(message)
        {

        }

    }
    public class AppCustomException_v2 : Exception
    {
        public string api_version => "2";
        public bool need_retry = false;

        public AppCustomException_v2() : base() { }

        public AppCustomException_v2(string message, bool need_retry = false) : base(message)
        {
            this.need_retry = need_retry;
        }
    }
}
