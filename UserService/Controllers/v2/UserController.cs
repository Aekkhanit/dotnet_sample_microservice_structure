using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Helpers;
using UserService.Models.v2;
using UserService.Module.v2;

namespace UserService.Controllers.v2
{
    [ApiController]
    [Authorize]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [EnableCors("AppPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserModule _IUserModule;
        private readonly IJwtHelper _IJwtHelper;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        public UserController(IUserModule IUserModule, IJwtHelper IJwtHelper, IHttpContextAccessor IHttpContextAccessor)
        {
            _IUserModule = IUserModule;
            _IJwtHelper = IJwtHelper;
            _IHttpContextAccessor = IHttpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login_Request request)
        {
            //declare your validation from body post
            var _valid = request != null && !string.IsNullOrEmpty(request.email) && !string.IsNullOrEmpty(request.email);
            if (_valid)
                return Ok(await _IJwtHelper.GenerateTokenAsync(request.email));
            return Unauthorized();
        }

        [HttpGet("user_profile")]
        public async Task<IActionResult> GetUserInfo()
        {
            var _email = _IHttpContextAccessor.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(_email))
                return Ok(await _IUserModule.GetUserInfoFromEmailAsync(_email));
            return Unauthorized();
        }


    }
}
