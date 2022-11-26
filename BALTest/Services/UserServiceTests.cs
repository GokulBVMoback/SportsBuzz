using API.Controllers;
using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using Moq;
using System.Collections.Generic;

namespace BALTest.Services
{
    public class UserServiceTests : IClassFixture<DataFixture>
    {
        DataFixture _fixture;
        UserService userService;
        Mock<IEncrypt> encrypt;

        public UserServiceTests(DataFixture fixture)
        {
            _fixture = fixture;
            encrypt = new Mock<IEncrypt>();
            userService = new UserService(_fixture.context, encrypt.Object);
        }

        [Fact]
        public void GetAll_User()
        {
            //Arrange

            //Act
            var result = userService.GetUser();

            //Assert
            var items = Assert.IsType<List<UserDisplay>>(result);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void Check_Extist_with_CheckExtistUser()
        {
            //Arrange
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "bvgokulgok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckExtistUser(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_New_with_CheckExtistUser()
        {
            //Arrange
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "anish@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckExtistUser(user);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Check_Correct_ConfirmPassword()
        {
            //Arrange
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "bvgokulgok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckPassword(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_Wrong_ConfirmPassword()
        {
            //Arrange
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "anish@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "4321", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckPassword(user);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Test_Register()
        {
            //Arrange
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "gok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword="1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.Registration(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void LogIn_with_correct_mail_password()
        {
            //Arrange
            var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "1234" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.LogIn(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void LogIn_with_correct_mail_wrong_password()
        {
            //Arrange
            var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "4321" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.LogIn(user);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void LogIn_with_new_mail()
        {
            //Arrange
            var user = new TblUser() { Email = "diptesh@gmail.com", Password = "4321" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.LogIn(user);
            
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Forget_Password_Test()
        {
            //Arrange
            var user = new Registration() { Email = "bvgokulgok@gmail.com", Password = "4321", ConfirmPassword="4321"};
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.ForgetPassword(user);

            //Assert
            Assert.True(result);
        }
    }
}