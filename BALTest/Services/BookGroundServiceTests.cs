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
        private readonly Mock<IGround> ground;

        public BookGroundServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            notification=new Mock<INotification>();
            ground=new Mock<IGround>(); 
            bookingGroundService=new BookingGroundService(_fixture.context, ground.Object,notification.Object);
        }

        [Fact]
        public void Get_GroundDetails()
        {
            //Arrange
            var bookground = new SearchAvailableGround()
            {
                SessionId = 1,
                City = "Chebnnai",
                Date = new DateTime(2022, 12, 19)
            };

            //Act
            var result = bookingGroundService.GetGroundDetails(bookground);
            //Assert
            var expected = _fixture.context.TblBookGrounds.Count();
            var items = Assert.IsType<List<GroundList>>(result);
            Assert.Equal(expected, items.Count);
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
            var result = bookingGroundService.BookingGround(booking);
            //Assert
           Assert.Null(result);
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
