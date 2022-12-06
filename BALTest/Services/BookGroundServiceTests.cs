using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
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
        private readonly BookingGroundService bookingGroundService;
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
        public void BookingGround_with_already_booked_Ground()
        {
            //Arrange
            var booking = new TblBookGround()
            {
                TeamId=1,
                SessionId=1,
                Date= new DateTime(2022, 12, 19),
                GroundId=1
            };

            //Act
            var result = bookingGroundService.CheckExtistBookedDetails(booking);
            //Assert
           Assert.True(result);
        }

        [Fact]
        public void GenerateMessage_notification()
        {
            //Arrage
            var message = new TblBookGround()
            {
                TeamId = 1,
                SessionId = 1,
                GroundId = 1
            };

            // Act
            var result = bookingGroundService.GenerateMessage(message);
            //Assert
            Assert.NotNull(result);   
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
        public void CheckExtist_booknewgrounddetails()
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
