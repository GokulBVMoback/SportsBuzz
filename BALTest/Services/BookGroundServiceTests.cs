using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Http.Features;
using Models.DbModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALTest.Services
{
    [Collection("Database collection")]

    public class BookGroundServiceTests
    {
        private readonly DataBaseFixture _fixture;
        private readonly IBookingGround bookingGroundService;
        private readonly Mock<INotification> notification;
        private readonly IGround ground;

        public BookGroundServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            notification = new Mock<INotification>();
            ground = new GroundService(_fixture.context);
            bookingGroundService = new BookingGroundService(_fixture.context, ground, notification.Object);
        }

        [Fact]
        public void Get_Available_GroundDetails_withAlreadyBooked_Session_Date()
        {
            //Arrange
            var bookground = new SearchAvailableGround()
            {
                SessionId = 1,
                City = "Chennai",
                Date = new DateTime(2022, 12, 19)
            };

            //Act
            var result = bookingGroundService.GetGroundDetails(bookground);
            //Assert
            var items = Assert.IsType<List<GroundList>>(result);
            Assert.Equal(0, items.Count);
        }

        [Fact]
        public void Get_Availabe_GroundDetails_not_booked_Session()
        {
            //Arrange
            var bookground = new SearchAvailableGround()
            {
                SessionId = 2,
                City="Chennai",
                Date = new DateTime(2022, 12, 19)
            };

            //Act
            var result = bookingGroundService.GetGroundDetails(bookground);
            //Assert
            var items = Assert.IsType<List<GroundList>>(result);
            Assert.Equal(1,items.Count);   
        }

        [Fact]
        public void Get_Availabe_GroundDetails_not_booked_date()
        {
            var bookground = new SearchAvailableGround()
            {
                SessionId=1,
                City="Chennai",
                Date= new DateTime(2022, 12, 20)
            };

            //Act
            var result = bookingGroundService.GetGroundDetails(bookground);
            //Assert
            var items = Assert.IsType<List<GroundList>>(result); 
            Assert.Equal(1, items.Count);
        }

        [Fact]
        public void checkExtist_bookgrounddetails()
        {
            //Arrange
            var bookdetailsExtist = new TblBookGround()
            {
                GroundId=1,
                SessionId=1,
                Date= new DateTime(2022, 12, 19)
            };

            //Act
            var result=bookingGroundService.CheckExtistBookedDetails(bookdetailsExtist);
            //Assert
            Assert.True(result);    
        }

        [Fact]
        public void CheckExtist_bookgrounddetails_groundId()
        {
            //Arrange
            var bookdetailsExtist = new TblBookGround()
            {
                GroundId=5,
                SessionId=4,
                Date=new DateTime(2022,12,19)
            };

            //Act
            var result = bookingGroundService.CheckExtistBookedDetails(bookdetailsExtist);
            //Assert
            Assert.False(result);   
        }
    }
}
