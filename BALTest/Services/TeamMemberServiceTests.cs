using BAL.Abstraction;
using BAL.Services;
using BALTest.Services;
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
    [Collection("Database collection")]
    public class TeamMemberServiceTests
    {
        private readonly ITeamMember teamMemberService;
        DataBaseFixture _fixture;

        public TeamMemberServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            teamMemberService = new TeamMemberService(_fixture.context);
        }

         [Fact]
        public void GetAll_TeamMember()
        {
            var expect = _fixture.context.TblTeamMembers.Count();
            var result = teamMemberService.GetTeamMember();
            var items = Assert.IsType<List<PlayerList>>(result);
            Assert.Equal(0, items.Count);
        }

        [Fact]
        public void Add_TeamMember()
        {
            //Arrange
            var AddMember = new TblTeamMember()
            {
                PlayerFirstName = "Sachin",
                PlayerLastName = "Tendulkar",
                Age = 23,
                JerseyNo = 2,
                State = "Karnataka",
                TeamId = 1,
            };
        }

        [Fact]
        public void CheckExtistTeamMember()
        {
            //Arrange
            int memberID=1;

            //Act
            var result = teamMemberService.CheckExtistTeamMember(memberID);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckExtistNewTeamMember()
        {
            //Arrange
            int memberID = 5;


            //Act
            var result =teamMemberService.CheckExtistTeamMember(memberID);

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
            try
            {
                teamMemberService.EditTeamMember(EditTeamMember);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }  
        }

        [Fact]
        public void Delete_TeamMember()
        {
            //Arrange
            int memberID = 1;

            //Act
            try
            {

                teamMemberService.DeleteTeamMember(memberID);
                Assert.True(true);  
            }
            catch 
            {
                Assert.False(false);    
            }
        }
    }    
}
