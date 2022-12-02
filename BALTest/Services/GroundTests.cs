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


namespace BALTest.Services
{

    [Collection("Database collection")]
    public class GroundTests
    {

        private readonly GroundService groundservice;
        private readonly DataBaseFixture _fixture;

        public GroundTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            groundservice = new GroundService(_fixture.context);
        }
        [Fact]
        public void Get_GroundDetails()
        {
            //Arrange
            var expect = _fixture.context.TblGrounds.Count();

            //Act
            var result = groundservice.GetGroundDetails();

            //Assert
            var items = Assert.IsType<List<GroundList>>(result);
            Assert.Equal(expect, items.Count);
        }
        [Fact]
        public void Add_Ground()
        {
            //Arrange
            var AddGround = new TblGround()
            {
                CompanyName= "india",
                Venue= "chinaswami",
                City="",
                Latitude="",
                Longitude="",
                SportType=1,
                UserId=2,
            };
        }
        [Fact]
        public void Edit_Ground()
        {
            //Arrange
            var Editground = new TblGround()
            {
                CompanyName = "bcci",
                Venue = "Lords",
                GroundId=1
            };

            //Act
            var result = groundservice.EditGround(Editground);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void SearchBy_GroundName()
        {
            //Arrange
            var GroundName = new TblGround()
            {
                Venue = "wankhere"
            };

            //Act
            var result = groundservice.SearchByGroundName(GroundName.Venue);

            //Assert
            var item = Assert.IsType<GroundList>(result);
            Assert.Equal(GroundName.Venue, result.Venue);
        }

        [Fact]
        public void SearchBy_GroundCity()
        {
            //Arrange
            var GroundCity = new TblGround()
            {
                City = "kolkata"
            };

            //Act
            var result = groundservice.SearchByGroundCity(GroundCity.City);
            //Assert
            var item = Assert.IsType<List<GroundList>>(result);
            Assert.Equal(1, item.Count);
        }
    }
}
