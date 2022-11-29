﻿using System;
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

namespace BALTests
{
    [CollectionDefinition("Team Service")]
    public class DataBaseTests : ICollectionFixture<DataBaseFixture>
    {

    }

    [Collection("Team Service")]
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
            Assert.Equal(2, items.Count);           
        } 

        [Fact]
        public void Team_serachByCity()
        {     
            //Arrange
            var city = new TblTeam()
            {
                City = "Benguluru"
            };
            //Act
            var result = _teamService.SearchByCity(city.City);
            //Assert
            Assert.True(city.City.Equals(city.City));
        }

        [Fact]
        public void SearchBy_TeamName()
        {
            //Arrange
            var teamName = new TblTeam()
            {
                TeamName = "Royal"
            };
            //Act
            var result =_teamService.SearchByTeamName(teamName.TeamName);
            //Assert
            Assert.True(teamName.TeamName.Equals(teamName.TeamName));            
        }

        [Fact]  
        public void CheckExtist_team()
        {
            //Arrange
            var teamExtist = new TblTeam()
            {
                TeamName="Royal"
            };
            //Act
            var result =_teamService.CheckExtistTeam(teamExtist);
            //Assert
            Assert.True(teamExtist.TeamId.Equals(teamExtist.TeamId));   
        }

        [Fact]  
        public void CheckExtist_userId()
        {
            //Arrange
            var teamExtistUser = new TblTeam()
            {
                UserId=1
            };
            //Act
            var result =_teamService.CheckExtistUserId(teamExtistUser);
            //Assert
            Assert.True(teamExtistUser.UserId.Equals(teamExtistUser.UserId));                          
        }

        [Fact]
        public void TeamRegistration_not()
        {
            //Arrange
            var teamRegister = new TblTeam()
            {                
                TeamName = "Royal",
                City = "Benguluru",
                SportType = 1,
                Email = "royal@gmail.com",
                PhoneNum = 83569879,
                UserId= 1              
            };
            //Act
            var result = _teamService.TeamRegistration(teamRegister);
            //Assert
            Assert.True(teamRegister.Equals(teamRegister));            
        }

        [Fact]
        public void Edit_teamName()
        {
            //Arrange
            var teamEdit = new TblTeam()
            {
                TeamId=1,   
            };
            //Act
            var result = _teamService.EditTeam(teamEdit);
            //Assert
            Assert.False(result.Equals(teamEdit.TeamId));            
        }

        [Fact]
        public void Delete_Team()
        {
            //Arrange
            var deleteteam = new TblTeam()
            {
                TeamId = 1,
            };
            //Act
            var result = _teamService.DeleteTeam(deleteteam);
            //Assert
            Assert.False(result.Equals(deleteteam));           
        }
    }
}