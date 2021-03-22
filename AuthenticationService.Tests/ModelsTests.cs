using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using AuthenticationService.Models;

namespace AuthenticationService.Tests
{
    public class ModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TokenHas1HourLifeTime()
        {
            var loginInfo = new LoginInfo("some@email.com");
            var expectedLifetime = DateTime.Now.AddHours(1);
            // comparing with epsilon
            Assert.AreEqual(0, (expectedLifetime - loginInfo.TokenLife).TotalMilliseconds);
        }

        [Test]
        public void UserInfoToStorageTest()
        {
            var profileName = "profileName";
            var registrationEmail = "registrationEmail";
            var password = "password";

            var userInfo = new UserInfo(profileName, registrationEmail, password);
            var expectedString = string.Join(StorageCommon.ElementSeparator, new[] { profileName, registrationEmail, password });
            Assert.AreEqual(expectedString, userInfo.ToStorageString());
        }

        [Test]
        public void LoginInfoToStorageTest()
        {
            var registrationEmail = "registrationEmail";
            var token = "token";
            var tokenLifetime = DateTime.Now.AddHours(1).ToString();

            var userInfo = new LoginInfo(registrationEmail, token);
            var expectedString = string.Join(StorageCommon.ElementSeparator, new[] { registrationEmail, token, tokenLifetime });
            Assert.AreEqual(expectedString, userInfo.ToStorageString());
        }

        public static IEnumerable<TestCaseData> UserInfoTestCases =>
            new List<TestCaseData>
            {
                new TestCaseData(true, 0),
                new TestCaseData(true, 1),
                new TestCaseData(true, 2),
                new TestCaseData(false, 3),
                new TestCaseData(true, 4),
            };

        public static IEnumerable<TestCaseData> LoginInfoTestCases =>
            new List<TestCaseData>
            {
                new TestCaseData(true, 0),
                new TestCaseData(true, 1),
                new TestCaseData(true, 2),
                new TestCaseData(false, 3),
                new TestCaseData(true, 4),
            };

        [TestCaseSource(nameof(UserInfoTestCases))]
        public void UserInfoCreateFromEnumerable(bool throws, int paramCount)
        {
            void creation() => new UserInfo(Enumerable.Repeat("", paramCount));
            if (throws)
                Assert.Throws(typeof(UserException), creation);
            else
                Assert.DoesNotThrow(creation);
        }

        [TestCaseSource(nameof(LoginInfoTestCases))]
        public void LoginInfoCreateFromEnumerable(bool throws, int paramCount)
        {
            void creation() => new LoginInfo(Enumerable.Repeat("", paramCount));
            if (throws)
                Assert.Throws(typeof(UserException), creation);
            else
            {
                var values = Enumerable.Repeat("", paramCount).ToArray();
                values[2] = DateTime.Now.ToString();
                Assert.DoesNotThrow(() => new LoginInfo(values));
            }
        }        
    }
}