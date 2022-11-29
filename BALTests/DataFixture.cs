using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALTests
{
    public class DataFixture : IDisposable
    {
        private static DbContextOptions<DbSportsBuzzContext> dbContextOptions = new DbContextOptionsBuilder<DbSportsBuzzContext>()
         .UseInMemoryDatabase(databaseName: "db_SportsBuzz")
            .Options;
        public DbSportsBuzzContext context;

        public DataFixture()
        {
            context = new DbSportsBuzzContext(dbContextOptions);
            context.Database.EnsureCreated();
            SeedDatadatabase();
        }

        public void SeedDatadatabase()
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

            var sports = new List<TblSportType>()
            {
                new TblSportType(){SportTypeId=1,SportType="Badminton"}
            };
            context.TblSportTypes.AddRange(sports);
            context.SaveChanges();

            var teams = new List<TblTeam>()
            {
                new TblTeam(){TeamId=1, TeamName="Royalk",City="Bengaluru",PhoneNum=8698789,Email="royalk@gmail.com", SportType=1,UserId=1,CreatedDate=null,UpdatedDate=null,Active=true},
                new TblTeam(){TeamId=2, TeamName="chennaisuperking",City="Chennai",PhoneNum=73582465,Email="Chennai@gmail.com",SportType=1,UserId = 2,CreatedDate=null,UpdatedDate=null,Active=true}
            };
            context.TblTeams.AddRange(teams);
            context.SaveChanges();

            var teamMembers = new List<TblTeamMember>()
            {
                new TblTeamMember(){MemberId=1,PlayerFirstName="Sachin",PlayerLastName="Tendulkar",Age=23,JerseyNo=2,State="Karnataka",TeamId=1}
            };
            context.TblTeamMembers.AddRange(teamMembers);
            context.SaveChanges();  
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
