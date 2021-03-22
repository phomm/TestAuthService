using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Routing;
using AuthenticationService.Models;
using AuthenticationService.Controllers;

namespace AuthenticationService.Tests
{
    public class UsersControllerTests
    {        
        private IUserRepository mUserRepository;
        private ILoginRepository mLoginRepository;
        private UsersController mUsersController;

        private static string ExceptionMessage => "request failed";
        private static string ZeroGuidToken => new Guid().ToString();
        private static UserException UserException => new UserException(ExceptionMessage);
        private static string ValidateRequestUrl =>
            $"/users/{UsersController.ValidateRequestEndpoint}?token={ZeroGuidToken}";

        [SetUp]
        public void Setup()
        {
            mUserRepository = Substitute.For<IUserRepository>();
            mLoginRepository = Substitute.For<ILoginRepository>();
            mUsersController = new UsersController(mUserRepository, mLoginRepository);
            mUsersController.Url = Substitute.For<UrlHelper>(Substitute.For<ActionContext>(
                Substitute.For<HttpContext>(), Substitute.For<RouteData>(), Substitute.For<ActionDescriptor>()));
            mUsersController.Url.Action(UsersController.ValidateRequestEndpoint, values: new { token = ZeroGuidToken })
                .ReturnsForAnyArgs(ValidateRequestUrl);                
        }

        public static IEnumerable<TestCaseData> AddUserTestCases =>
            new List<TestCaseData>
            {
                new TestCaseData((Func<CallInfo, bool>)(x => true), new JsonResult(new ApiResponse(true))),
                new TestCaseData((Func<CallInfo, bool>)(x => false ? true : throw UserException), 
                    new NotFoundObjectResult(new ApiResponse(null) { Message = ExceptionMessage })),
            };

        public static IEnumerable<TestCaseData> RequestPasswordTestCases =>
            new List<TestCaseData>
            {
                new TestCaseData((Func<CallInfo, string>)(x => ZeroGuidToken), 
                    new JsonResult(new ApiResponse("/api" + ValidateRequestUrl))),
                new TestCaseData((Func<CallInfo, string>)(x => false ? "" : throw UserException),
                    new NotFoundObjectResult(new ApiResponse(null) { Message = ExceptionMessage })),
            };

        [TestCaseSource(nameof(AddUserTestCases))]
        public void AddUser(Func<CallInfo, bool> argument, IActionResult expected)
        {
            mUserRepository.AddUser(new UserInfo()).ReturnsForAnyArgs(argument);            
            
            var result = mUsersController.AddUser(new UserInfo());

            Assert.True(Equals(expected.ToValue(), result.ToValue()));
        }

        [TestCaseSource(nameof(RequestPasswordTestCases))]
        public void RequestPassword(Func<CallInfo, string> argument, IActionResult expected)
        {
            mLoginRepository.AddLoginRequest("").ReturnsForAnyArgs(argument);
            
            var result = mUsersController.RequestUserPassword("");

            Assert.True(Equals(expected.ToValue(), result.ToValue()));
        }
    }
}