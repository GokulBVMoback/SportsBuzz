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
        private readonly BookingGroundService _bookinggroundservice;
        private readonly DataBaseFixture _fixture;
        private readonly Mock<INotification> notification;

        public BookGroundServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            notification= new Mock<INotification>();
            _bookinggroundservice = new BookingGroundService(_fixture.context,notification.Object);
        }

        [Fact]
        public void Get_GroundDetails()
        {
            
            
        }







    }
}
