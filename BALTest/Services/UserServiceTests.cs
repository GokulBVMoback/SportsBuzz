using API.Controllers;
using BAL.Abstraction;
using BAL.Services;
using Castle.Components.DictionaryAdapter.Xml;
using Entities.Models;
using EnvDTE;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.DbModels;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace BALTest.Services
{
    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DataBaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    [Collection("Database collection")]
    public class UserServiceTests
    {
        private readonly DataBaseFixture _fixture;
        private readonly UserService userService;
        private readonly Mock<IEncrypt> encrypt;
        private readonly Mock<IGenarate> genarate;


        public UserServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            encrypt = new Mock<IEncrypt>();
            genarate = new Mock<IGenarate>();
            userService = new UserService(_fixture.context, encrypt.Object,genarate.Object);

        }

        [Fact]
        public void GetAll_User()
        {
            //Arrange

            //Act
            var result = userService.GetUser();

            //Assert
            var expected=_fixture.context.TblUsers.Count();
            var items = Assert.IsType<List<UserDisplay>>(result);
            Assert.Equal(expected, items.Count);
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
            genarate.Setup(x=>x.GenerateToken(user)).Returns("login successfull");

            //Act
            var result = userService.Registration(user);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LogIn_with_correct_mail_password()
        {
            //Arrange
            var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "1234" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);
            genarate.Setup(x => x.GenerateToken(user)).Returns("login successfull");
            //Act
            string result = userService.LogIn(user);

            //Assert
            Assert.NotNull(result);
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
            Assert.Equal(null,result);
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
            Assert.Equal(null,result);
        }

        [Fact]
        public void Forget_Password_Test()
        {
            //Arrange
            var user = new Registration() { Email = "bvgokulgok@gmail.com", Password = "4321", ConfirmPassword="4321"};
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            try
            {
                userService.ForgetPassword(user);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }

            //Assert
           
        }
    }
}
