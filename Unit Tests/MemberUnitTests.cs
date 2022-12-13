using System.Reflection;
using Application.DAOInterfaces;
using Application.Logic;
using Data.DAOs;
using Domain.DTOs.Member;
using Domain.Models;

namespace Unit_Tests
{
    [TestClass]
    public class MemberUnitTests
    {
        [TestMethod]
        public void ValidateFullNameEmpty()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "",
                Email = "test@test.com",
                Password = "Test123!",
                UserName = "test "
            };
            var expectedErrorMessage = "Full name must not be empty";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidateFullNameSpecialCharacters()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test!%",
                Email = "test@test.com",
                Password = "Test123!",
                UserName = "test "
            };
            var expectedErrorMessage = "Full name: \n- should consist only of latin alphabet letters (A-Z, a-z)\n- should not contain any special characters (!,@,#,$,...) or digits\n- should be in format \"FirstName LastName\"";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidateEmailEmpty()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "",
                Password = "Test123!",
                UserName = "test "
            };
            var expectedErrorMessage = "Email must not be empty";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidateEmailFormat()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test.test",
                Password = "Test123!",
                UserName = "test "
            };
            var expectedErrorMessage = "Please input correct email.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidatePasswordEmpty()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "",
                UserName = "test "
            };
            var expectedErrorMessage = "Password must not be empty";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidatePasswordSpecialChar()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "Test123",
                UserName = "test "
            };
            var expectedErrorMessage = "Password must contain at least one special character.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidatePasswordDigit()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "Test!",
                UserName = "test "
            };
            var expectedErrorMessage = "Password must contain at least one digit.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidatePasswordOneUpper()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "test123!",
                UserName = "test "
            };
            var expectedErrorMessage = "Password must contain at least one upper case character.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidateUsernameEmpty()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "Test123!",
                UserName = ""
            };
            var expectedErrorMessage = "Username must not be empty";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }

        [TestMethod]
        public void ValidateUsernameLength()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "Test123!",
                UserName = "usr"
            };
            var expectedErrorMessage = "Username must be between 5 and 20 characters.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);
        }
        
        //TODO review need for this function
        [TestMethod]
        public void ValidateId()
        {
            IMemberDAO memberDAO = new MemberDAO();
            MemberLogic logic = new MemberLogic(memberDAO);

            MemberCreateDTO MemberCreateDTO = new MemberCreateDTO()
            {
                FullName = "Test Test",
                Email = "test@test.com",
                Password = "Test123!",
                UserName = "usr"
            };
            var expectedErrorMessage = "Username must be between 5 and 20 characters.";
            Task<Exception> ex = Assert.ThrowsExceptionAsync<Exception>(() => logic.CreateAsync(MemberCreateDTO));
            Assert.AreEqual(expectedErrorMessage, ex.Result.Message);

        }

    }
}