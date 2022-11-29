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
                new TblUser(){ UserId=3, FirstName="Gokul", LastName="B V", Email="bvgok@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true }
            };
            context.TblUsers.AddRange(user);
            context.SaveChanges();

            var sport = new List<TblSportType>()
            {
                new TblSportType(){ SportTypeId=1, SportType="Badmindon"  },
                new TblSportType(){ SportTypeId=2, SportType="Tennis" }
            };
            context.TblSportTypes.AddRange(sport);
            context.SaveChanges();

            var team = new List<TblTeam>()
            {
                new TblTeam(){ TeamId=1, UserId=1, TeamName="MKP Stars", City="Nagercoil", Email="bvgokulgok@gmail.com", PhoneNum=8834834383,  SportType=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                //new TblTeam(){ UserId=2, FirstName="Gokul", LastName="B V", Email="bvgokul@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=2, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true },
                //new TblTeam(){ UserId=3, FirstName="Gokul", LastName="B V", Email="bvgok@gmail.com", PhoneNum=8834834383, Password="1234", UserRole=1, CreatedDate=DateTime.Now, UpdatedDate=null, Active=true }
            };
            context.TblTeams.AddRange(team);
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
