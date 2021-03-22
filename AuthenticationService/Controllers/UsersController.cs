using AuthenticationService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        public const string ValidateRequestEndpoint = "ValidateRequest";
        public const string RequestPasswordEndpoint = "RequestPassword";

        private IUserRepository mUserRepository;
        private ILoginRepository mLoginRepository;

        public UsersController(IUserRepository userRepository, ILoginRepository loginRepository)
        {
            mUserRepository = userRepository;
            mLoginRepository = loginRepository;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var result = mUserRepository.GetUsers();
            return Json(new ApiResponse(result));
        }

        [HttpGet("requests")]
        public IActionResult GetRequests()
        {
            var result = mLoginRepository.GetLoginRequests();
            return Json(new ApiResponse(result));
        }

        [HttpPost("add")]
        public IActionResult AddUser([FromBody] UserInfo userInfo)
        {
            bool result;
            try
            {
                result = mUserRepository.AddUser(userInfo);
            }
            catch (UserException ex)
            {
                return NotFound(new ApiResponse(null) { Message = ex.Message });
            }
            
            return Json(new ApiResponse(result));
        }

        [HttpPost(RequestPasswordEndpoint)]
        public IActionResult RequestUserPassword([FromBody] string email)
        {
            string result;
            try
            {
                result = mLoginRepository.AddLoginRequest(email);
            }
            catch (UserException ex)
            {
                return NotFound(new ApiResponse(null) { Message = ex.Message });
            }
            var url = Url.Action(ValidateRequestEndpoint, values: new { token = result});
            var apiUrl = "/api" + url;
            var letter = MailComposer.ComposeChangePasswordLetter(apiUrl);
            // todo sending mail, for now just showing the letter, but by design apiUrl is to be returned
            return Json(new ApiResponse(apiUrl));
        }

        [HttpGet(ValidateRequestEndpoint)]
        public IActionResult ValidateLoginRequest([FromQuery] string token)
        {
            var result = mLoginRepository.ValidateLoginRequest(token);
            // LoginByEmailAddress();
            // we should send a logged in account View (from a controller responsible for Views, like Home/Account)
            // but for now only response about success/fail
            return Json(new ApiResponse(result.ToString()));
        }


    }
}

