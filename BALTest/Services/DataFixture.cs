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
    public class DataFixture:IDisposable
    {
        private static DbContextOptions<DbSportsBuzzContext> dbContextOptions = new DbContextOptionsBuilder<DbSportsBuzzContext>()
          .UseInMemoryDatabase(databaseName: "db_SportsBuzz")
          .Options;
        public DbSportsBuzzContext context;

        public DataFixture()
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
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
