using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using NHibernate.Mapping;
using Repository;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingGroundController : BaseController
    {
        private readonly IBookingGround _bookingGround;
        private readonly CrudStatus crudStatus;

        public BookingGroundController(DbSportsBuzzContext dbcontext, IBookingGround bookingGround) : base(dbcontext)
        {
            _bookingGround = bookingGround;
            crudStatus = new CrudStatus();
        }

        [HttpPost("GetAvailableGroundDetails")]
        [Authorize]
        public JsonResult GetAvailableGroundDetails(int userId, SearchAvailableGround availableGround)
        {
            try
            {
                return new JsonResult(_bookingGround.GetGroundDetails(userId,availableGround).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("Booking_Match")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult BookingGround(GroundBooking booking)
        {
            try
            {
                var bookingdto=AutoMapper<GroundBooking, TblBookGround>.MapClass(booking);

                bool result = _bookingGround.CheckExtistBookedDetails(bookingdto);
                if (result == false)
                {
                    _bookingGround.BookingGround(bookingdto);
                    crudStatus.Status = true;
                    crudStatus.Message = "Ground booked successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "This Ground already booked at this date and session";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
