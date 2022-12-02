using System;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Entities.Models;
using Xunit;
using BAL.Abstraction;
using BAL.Services;
using Models.DbModels;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Numerics;
using EnvDTE;
using Xunit.Abstractions;
using BALTest.Services;

namespace BALTests
{

    [Collection("Database collection")]
    public class TeamserviceTests
    { 
        private readonly TeamService _teamService;
        DataBaseFixture _fixture;

        public TeamserviceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            _teamService = new TeamService(_fixture.context);
        }
   
        [Fact]
        public void GetAll_Team()
        {
            //Arrange

            //Act
            var result = _teamService.GetTeam();

            //Assert
            var items = Assert.IsType<List<TeamList>>(result);
            Assert.Equal(3, items.Count);           
        } 

        [Fact]
        public void Search_By_City()
        {     
            //Arrange
            var city = new TblTeam()
            {
                City = "Chennai"
            };

            //Act
            var result = _teamService.SearchByCity(city.City);

            //Assert
            Assert.Equal(1,result.Count());
        }

        [Fact]
        public void Search_By_TeamName()
        {
            //Arrange
            var teamName = new TblTeam()
            {
                TeamName = "Royalk"
            };

            //Act
            var result =_teamService.SearchByTeamName(teamName.TeamName);

            //Assert
            Assert.Equal(teamName.TeamName,result.TeamName);            
        }

        [Fact]  
        public void CheckExtistTeam_Extist_Team()
        {
            //Arrange
            var teamExtist = new TblTeam()
            {
                TeamName="Royalk"
            };

            //Act
            var result =_teamService.CheckExtistTeam(teamExtist);

            //Assert
            Assert.True(result);   
        }

        [Fact]
        public void CheckExtistTeam_New_Team()
        {
            //Arrange
            var teamExtist = new TblTeam()
            {
                TeamName = "Royalkk"
            };

            //Act
            var result = _teamService.CheckExtistTeam(teamExtist);

            //Assert
            Assert.False(result);
        }

        [Fact]  
        public void CheckExtist_UserId_Already_Extist()
        {
            //Arrange
            var teamExtistUser = new TblTeam()
            {
                UserId=1
            };

            //Act
            var result =_teamService.CheckExtistUserId(teamExtistUser);

            //Assert
            Assert.True(result);                          
        }

        [Fact]
        public void CheckExtist_UserId_Already_New()
        {
            //Arrange
            var teamExtistuser = new TblTeam()
            {
                UserId = 5,
            };

            //Act
            var result = _teamService.CheckExtistUserId(teamExtistuser);

            //Assert
            Assert.False(result);  
        }

        [Fact]
        public void Check_TeamRegistration()
        {
            //Arrange
            var teamRegister = new TblTeam()
            {                
                TeamName = "CSK",
                City = "Chennai",
                SportType = 1,
                Email = "royal@gmail.com",
                PhoneNum = 83569879,
                UserId= 3              
            };

            //Act
            var result = _teamService.TeamRegistration(teamRegister);

            //Assert
            Assert.True(result);            
        }

        [Fact]
        public void Edit_teamName()
        {
            //Arrange
            var teamEdit = new TblTeam()
            {
                TeamId = 3
            };

            //Act
            var result = _teamService.EditTeam(teamEdit);

            //Assert
            Assert.True(result);            
        }

        [Fact]
        public void Delete_Team()
        {
            //Arrange
            var deleteteam = new TblTeam()
            {
                TeamId = 2,
            };

            //Act
            var result = _teamService.DeleteTeam(deleteteam);

            //Assert
            Assert.True(result);           
        }
    }
}
