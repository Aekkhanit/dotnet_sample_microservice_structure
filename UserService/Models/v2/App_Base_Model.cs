using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.v2
{



    public class Base_Response
    {
        /// <summary>
        /// v2 Base response
        /// </summary>
        public Base_Response(bool success
            , bool need_retry = false
            , object content = null
            , string description = null)
        {
            this.content = content;
            this.success = success;
            this.need_retry = need_retry;
            this.description = description;
        }
        public bool success { get; set; }
        public bool need_retry { get; set; }
        public object content { get; set; }
        public string description { get; set; }
    }
}
