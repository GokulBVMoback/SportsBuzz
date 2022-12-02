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

        public BookingGroundController(DbSportsBuzzContext dbcontext, IBookingGround bookingGround) : base(dbcontext)
        {
            _bookingGround = bookingGround;
        }

        [HttpPost("GetAvailableGroundDetails")]
        //[Authorize]
        public JsonResult GetGroundDetails(SearchAvailableGround availableGround)
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
        public JsonResult TeamRegistration(TblBookGround booking)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _bookingGround.CheckExtistBookedDetails(booking);
                if (result == false)
                {
                    result = _bookingGround.BookingGround(booking);
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
