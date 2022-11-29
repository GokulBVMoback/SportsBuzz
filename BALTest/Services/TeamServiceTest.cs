using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Models.DbModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALTest.Services
{

    [Collection("Database collection")]
    public class TeamServiceTest
    {
        private readonly DataBaseFixture _fixture;
        private readonly TeamService userService;

        public TeamServiceTest(DataBaseFixture fixture)
        {
            _fixture = fixture;
            userService = new TeamService(_fixture.context);
        }

        [Fact]
        public void GetAll_Team()
        {
            //Arrange

            //Act
            var result = userService.GetTeam();

            //Assert
            var expected = _fixture.context.TblTeams.Count();
            var items = Assert.IsType<List<TeamList>>(result);
            Assert.Equal(1, items.Count);
        }

        //[Fact]
        //public void Check_Extist_with_CheckExtistUser()
        //{
        //    //Arrange
        //    var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "bvgokulgok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

        //    //Act
        //    var result = userService.CheckExtistUser(user);

        //    //Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void Check_New_with_CheckExtistUser()
        //{
        //    //Arrange
        //    var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "anish@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

        //    //Act
        //    var result = userService.CheckExtistUser(user);

        //    //Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public void Check_Correct_ConfirmPassword()
        //{
        //    //Arrange
        //    var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "bvgokulgok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

        //    //Act
        //    var result = userService.CheckPassword(user);

        //    //Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void Check_Wrong_ConfirmPassword()
        //{
        //    //Arrange
        //    var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "anish@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "4321", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };

        //    //Act
        //    var result = userService.CheckPassword(user);

        //    //Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public void Test_Register()
        //{
        //    //Arrange
        //    var user = new Registration() { FirstName = "Gokul", LastName = "B V", Email = "gok@gmail.com", PhoneNum = 8834834383, Password = "1234", ConfirmPassword = "1234", UserRole = 1, CreatedDate = DateTime.Now, UpdatedDate = null, Active = true };
        //    encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

        //    //Act
        //    var result = userService.Registration(user);

        //    //Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void LogIn_with_correct_mail_password()
        //{
        //    //Arrange
        //    var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "1234" };
        //    encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

        //    //Act
        //    var result = userService.LogIn(user);

        //    //Assert
        //    Assert.True(result);
        //}

        //[Fact]
        //public void LogIn_with_correct_mail_wrong_password()
        //{
        //    //Arrange
        //    var user = new TblUser() { Email = "bvgokulgok@gmail.com", Password = "4321" };
        //    encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

        //    //Act
        //    var result = userService.LogIn(user);

        //    //Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public void LogIn_with_new_mail()
        //{
        //    //Arrange
        //    var user = new TblUser() { Email = "diptesh@gmail.com", Password = "4321" };
        //    encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

        //    //Act
        //    var result = userService.LogIn(user);

        //    //Assert
        //    Assert.False(result);
        //}

        //[Fact]
        //public void Forget_Password_Test()
        //{
        //    //Arrange
        //    var user = new Registration() { Email = "bvgokulgok@gmail.com", Password = "4321", ConfirmPassword = "4321" };
        //    encrypt.Setup(method => method.EncodePasswordToBase64(user.Password)).Returns(user.Password);

        //    //Act
        //    var result = userService.ForgetPassword(user);

        //    //Assert
        //    Assert.True(result);
        //}
    }
}