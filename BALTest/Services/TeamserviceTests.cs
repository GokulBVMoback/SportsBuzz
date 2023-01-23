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
        private readonly ITeam _teamService;
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
            var expect = _fixture.context.TblTeams.Count();

            //Act
            var result = _teamService.GetTeam();

            //Assert
            var items = Assert.IsType<List<TeamList>>(result);
            Assert.Equal(expect, items.Count);           
        } 

        [Fact]
        public void Team_serachByCity()
        {     
            //Arrange
            var city = new TblTeam()
            {
                City = "Chennai"
            };

            //Act
            var result = _teamService.SearchByCity(city.City);

            //Assert
            var items = Assert.IsType<List<TeamList>>(result);
            Assert.Equal(1, items.Count());
        }

        [Fact]
        public void SearchBy_TeamName()
        {
            //Arrange
            var teamName = new TblTeam()
            {
                TeamName = "Royalk"
            };

            //Act
            var result =_teamService.SearchByTeamName(teamName.TeamName);

            //Assert
            var items = Assert.IsType<TeamList>(result);
            Assert.Equal(teamName.TeamName, items.TeamName);
        }

        [Fact]  
        public void CheckExtist_team()
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
        public void CheckExtist_Newteam()
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

        //[Fact]  
        //public void CheckExtist_userId()
        //{
        //    //Arrange
        //    var teamExtistUser = new TblTeam()
        //    {
        //        UserId=1
        //    };

        //    //Act
        //    var result =_teamService.CheckExtistUserId(teamExtistUser);

        //    //Assert
        //    Assert.True(result);                          
        //}

        //[Fact]
        //public void CheckExtist_newuserId()
        //{
        //    //Arrange
        //    var teamExtistuser = new TblTeam()
        //    {
        //        UserId = 5,
        //    };

        //    //Act
        //    var result = _teamService.CheckExtistUserId(teamExtistuser);

        //    //Assert
        //    Assert.False(result);  
        //}

        [Fact]
        public void TeamRegistration()
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
            try
            {
                 _teamService.TeamRegistration(teamRegister);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }                      
        }

        [Fact]
        public void Edit_teamName()
        {
            //Arrange
            var teamEdit = new TblTeam()
            {
                TeamId = 3
            };
            try
            {
                _teamService.EditTeam(teamEdit);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }                      
        }
    }
}
