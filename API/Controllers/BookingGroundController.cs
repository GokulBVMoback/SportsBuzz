using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;


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
        public JsonResult GetAvailableGroundDetails(SearchAvailableGround availableGround)
        {
            try
            {
                return new JsonResult(_bookingGround.GetGroundDetails(availableGround).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("Booking_Match")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult BookingGround(TblBookGround booking)
        {
            try
            {
                bool result = _bookingGround.CheckExtistBookedDetails(booking);
                if (result == false)
                {
                    _bookingGround.BookingGround(booking);
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
