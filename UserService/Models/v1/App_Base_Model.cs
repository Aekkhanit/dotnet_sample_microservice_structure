using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models.v1
{
    public enum Base_Reponse_Status
    {
        success = 200,
        invalid_paramteer = 300,
        bad_request = 400,
        server_exception = 911,
        no_information = 204
    }


    public class Base_Response
    {
        /// <summary>
        /// v1 Base response
        /// </summary>
        public Base_Response(Base_Reponse_Status code_status
            , object content = null
            , string description = null)
        {
            this.content = content;
            this.code = code_status;
            this.description = description;
        }
        public Base_Reponse_Status code { get; set; }
        public object content { get; set; }
        public string description { get; set; }
    }
}
