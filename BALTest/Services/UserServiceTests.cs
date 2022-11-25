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
    public class UserServiceTests
    {
        private static DbContextOptions<DbSportsBuzzContext> dbContextOptions = new DbContextOptionsBuilder<DbSportsBuzzContext>()
                  .UseInMemoryDatabase(databaseName: "db_SportsBuzz")
                  .Options;
        DbSportsBuzzContext context;
        UserService userService;
        Mock<IEncrypt> encrypt;

        public UserServiceTests()
        {
            context = new DbSportsBuzzContext(dbContextOptions);
            context.Database.EnsureCreated();
            encrypt = new Mock<IEncrypt>();
            userService = new UserService(context, encrypt.Object);
        }

        public void SeedDatabase()
        {
            var role = new List<TblUserRole>()
            {
                new TblUserRole(){ UserRoleId=1, UserRole="Team Manager"  },
                new TblUserRole(){ UserRoleId=2, UserRole="Ground Manager" }
            };
            context.TblUserRoles.AddRange(role);
            context.SaveChanges();

            var user = new List<TblUser>()
            {
                new TblUser(){ UserId=1, FirstName="Gokul", LastName="B V", Email="bvgokulgok@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TblUser(){ UserId=2, FirstName="Gokul", LastName="B V", Email="bvgokul@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=2, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TblUser(){ UserId=3, FirstName="Gokul", LastName="B V", Email="bvgok@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true }
            };
            context.TblUsers.AddRange(user);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetAll_User()
        {
            //Arrange
            SeedDatabase();

            //Act
            var result = userService.GetUser();

            //Assert
            var items = Assert.IsType<List<UserDisplay>>(result);
            Assert.Equal(3, items.Count);
            
            Dispose();
        }

        [Fact]
        public void Check_Extist_with_CheckExtistUser()
        {
            //Arrange
            SeedDatabase();
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "bvgokulgok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckExtistUser(user);

            //Assert
            Assert.True(result);

            Dispose();
        }

        [Fact]
        public void Check_New_with_CheckExtistUser()
        {
            //Arrange
            SeedDatabase();
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "anish@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckExtistUser(user);

            //Assert
            Assert.False(result);

            Dispose();
        }

        [Fact]
        public void Check_Correct_ConfirmPassword()
        {
            //Arrange
            SeedDatabase();
            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "bvgokulgok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckPassword(user);

            //Assert
            Assert.True(result);

            Dispose();
        }

        [Fact]
        public void Check_Wrong_ConfirmPassword()
        {
            //Arrange
            SeedDatabase();

            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "anish@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "4321", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

            //Act
            var result = userService.CheckPassword(user);

            //Assert
            Assert.False(result);

            Dispose();
        }

        [Fact]
        public void Test_Register()
        {
            //Arrange
            SeedDatabase();

            var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "gok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword="1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.Registration(user);

            //Assert
            Assert.True(result);

            Dispose();
        }

        [Fact]
        public void LogIn_with_correct_mail_password()
        {
            //Arrange
            SeedDatabase();
            var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "1234" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.LogIn(user);

            //Assert
            Assert.True(result);

            Dispose();
        }

        [Fact]
        public void LogIn_with_correct_mail_wrong_password()
        {
            //Arrange
            SeedDatabase();
            var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "4321" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.LogIn(user);

            //Assert
            Assert.False(result);

            Dispose();
        }

        [Fact]
        public void LogIn_with_new_mail()
        {
            //Arrange
            SeedDatabase();
            var user = new TblUser() { Email = "diptesh@gmail.com", Password = "4321" };
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.LogIn(user);
            
            //Assert
            Assert.False(result);

            Dispose();
        }

        [Fact]
        public void Forget_Password_Test()
        {
            //Arrange
            SeedDatabase();
            var user = new Registration() { Email = "bvgokulgok@gmail.com", Password = "4321", ConfirmPassword="4321"};
            encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

            //Act
            var result = userService.ForgetPassword(user);

            //Assert
            Assert.True(result);

            Dispose();
        }
    }
}