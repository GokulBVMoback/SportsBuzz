using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALTest.Services
{
    public class DataBaseFixture:IDisposable
    {
        private static DbContextOptions<DbSportsBuzzContext> dbContextOptions = new DbContextOptionsBuilder<DbSportsBuzzContext>()
          .UseInMemoryDatabase(databaseName: "db_SportsBuzz")
          .Options;
        public DbSportsBuzzContext context;

        public DataBaseFixture()
        {
            context = new DbSportsBuzzContext(dbContextOptions);
            context.Database.EnsureCreated();
            SeedDatabase();
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
                new TblUser(){ UserId=3, FirstName="Gokul", LastName="B V", Email="bvgok@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                new TblUser(){ UserId=4, FirstName="Gokul", LastName="B V", Email="bv@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true }
            };
            context.TblUsers.AddRange(user);
            context.SaveChanges();

            var sports = new List<TblSportType>()
            {
                new TblSportType(){SportTypeId=1,SportType="Badminton"},
                new TblSportType(){SportTypeId=2,SportType="Tennis"}
            };
            context.TblSportTypes.AddRange(sports);
            context.SaveChanges();

            var teams = new List<TblTeam>()
            {
                new TblTeam(){TeamId=1, TeamName="Royalk",City="Bengaluru",PhoneNum=8698789,Email="royalk@gmail.com", SportType=1,UserId=1,CreatedDate=null,UpdatedDate=null,Active=true},
                new TblTeam(){TeamId=2, TeamName="chennaisuperking",City="Chennai",PhoneNum=73582465,Email="Chennai@gmail.com",SportType=1,UserId = 3,CreatedDate=null,UpdatedDate=null,Active=true},
                new TblTeam(){TeamId=3, TeamName="KKR",City="Kolkata",PhoneNum=73582465,Email="kolkata@gmail.com",SportType=1,UserId = 4,CreatedDate=null,UpdatedDate=null,Active=true}

            };
            context.TblTeams.AddRange(teams);
            context.SaveChanges();

            var teamMembers = new List<TblTeamMember>()
            {
                new TblTeamMember(){MemberId=1,PlayerFirstName="Sachin",PlayerLastName="Tendulkar",Age=23,JerseyNo=2,State="Karnataka",TeamId=1}
            };
            context.TblTeamMembers.AddRange(teamMembers);
            context.SaveChanges();

            var Ground = new List<TblGround>()
            {
                new TblGround(){GroundId=1, CompanyName="india",Venue="chinaswami",City="chennai",Latitude="12345",Longitude="1234", SportType=1,UserId=2,CreatedDate=null,UpdatedDate=null,Active=true},
                new TblGround(){GroundId=2, CompanyName="Dhinka",Venue="wankhere",City="Mumbai",Latitude="4567",Longitude="98765", SportType=2,UserId=2,CreatedDate=null,UpdatedDate=null,Active=true},
                new TblGround(){GroundId=3, CompanyName="bengal",Venue="hula",City="Kolkata",Latitude="6789",Longitude="69587", SportType=1,UserId=2,CreatedDate=null,UpdatedDate=null,Active=true},
                new TblGround(){GroundId=4, CompanyName="Brila",Venue="Modi",City="Gujarat",Latitude="4567",Longitude="98765", SportType=2,UserId=2,CreatedDate=null,UpdatedDate=null,Active=true}
            };
            context.TblGrounds.AddRange(Ground);
            context.SaveChanges();
        }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
