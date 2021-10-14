using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Helpers;
using UserService.Models.v1;
using UserService.Module.v1;

namespace UserService.Controllers.v1
{
    [ApiController]
    [Authorize]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> GetUserInfo([FromBody] Login_Request request)
        {
            //declare your validation from body post
            var _valid = request != null && !string.IsNullOrEmpty(request.username) && !string.IsNullOrEmpty(request.password);
            var token = await _IJwtHelper.GenerateTokenAsync(request.username);
            if (_valid)
                return Ok(token);
            return Unauthorized();
        }

        [HttpPost("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var _username = _IHttpContextAccessor.HttpContext.User.Identity.Name;
            if (!string.IsNullOrEmpty(_username))
                return Ok(await _IUserModule.GetUserInfoByUsernameAsync(_username));
            return Unauthorized();
        }
    }
}
