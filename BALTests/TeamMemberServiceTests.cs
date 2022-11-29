using BAL.Services;
using Entities.Models;
using EnvDTE;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALTests
{
    [Collection("Team Service")]
    public class TeamMemberServiceTests
    {
        private readonly TeamMemberService teamMemberService;
        DataBaseFixture _fixture;

        public TeamMemberServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            teamMemberService = new TeamMemberService(_fixture.context);
        }

         [Fact]
        public void GetAll_TeamMember()
        {
            var result = teamMemberService.GetTeamMember();
            var items = Assert.IsType<List<PlayerList>>(result);
            Assert.Equal(1, items.Count);
        }

        [Fact]
        public void Add_TeamMember()
        {
            //Arrange
            var AddMember = new TblTeamMember()
            {
                MemberId = 1,
                PlayerFirstName = "Sachin",
                PlayerLastName = "Tendulkar",
                Age = 23,
                JerseyNo = 2,
                State = "Karnataka",
                TeamId = 1,
            };
            //Act
            var result = teamMemberService.AddTeamMember(AddMember);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void TeamMember_Extist()
        {
            //Arrange
            var teamMemberExtist = new TblTeamMember()
            {
                MemberId = 1,
            };
            //Act
            var result = teamMemberService.TeamMemberExtist(teamMemberExtist);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void TeamMember_newExtits()
        {
            //Arrange
            var teamMemberExtist = new TblTeamMember()
            {
                MemberId = 5,
            };
            //Act
            var result =teamMemberService.TeamMemberExtist(teamMemberExtist);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void Edit_TeamMember()
        {
            //Arrange
            var EditTeamMember = new TblTeamMember()
            {
                MemberId = 1,
            };
            //Act
            var result = teamMemberService.EditTeamMember(EditTeamMember);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_TeamMember()
        {
            //Arrange
            var DeleteTeamMember = new TblTeamMember()
            {
                MemberId = 1,
            };
              //Act
            var result = teamMemberService.DeleteTeamMember(DeleteTeamMember);
            //Assert
            Assert.True(result);
        }
    }    
}
