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
    ///<summary>
    ///[ApiController]attribute makes model validation errors automatically trigger an HTTP 400 response
    ///</summary>
    
    public class BookingGroundController : BaseController
    {
        /// <summary>
        /// By the dependency injection we are calling all the methods 
        /// </summary>

        private readonly IBookingGround _bookingGround;
        private readonly CrudStatus crudStatus;

        /// <summary>
        /// In this constructor we are passing these parameters
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="bookingGround"></param>
        public BookingGroundController(DbSportsBuzzContext dbcontext, IBookingGround bookingGround) : base(dbcontext)
        {
            _bookingGround = bookingGround;
            crudStatus = new CrudStatus();
        }

        /// <summary>
        /// calling GetGroundDetails() from the BookGroundService
        /// </summary>
        /// <param name="availableGround"></param>
        /// <returns></returns>
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

        /// <summary>
        /// calling BookingGround() from the BookGroundService
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
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
