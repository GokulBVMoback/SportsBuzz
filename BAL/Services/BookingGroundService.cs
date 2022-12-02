using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BookingGroundService:IBookingGround
    {
        private readonly DbSportsBuzzContext _dbContext;
        GroundService ground;

        public BookingGroundService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
            ground = new GroundService(_dbContext);
        }

        public List<GroundList> GetGroundDetails(SearchAvailableGround availableGround)
        {
            List<TblBookGround> list = _dbContext.TblBookGrounds.ToList().Where(x => x.SessionId == availableGround.SessionId && x.Date==availableGround.Date).ToList();
            List<GroundList> grd2 = ground.SearchByGroundCity(availableGround.City).ToList();            
            foreach (var items in list)
            {
                grd2 = grd2.Where(x => x.GroundId != items.GroundId).ToList();
            }            
            return grd2;
        }

        public bool BookingGround(TblBookGround booking)
        {
            _dbContext.TblBookGrounds.Add(booking);
            _dbContext.SaveChanges();
            return true;
        }

        public bool CheckExtistBookedDetails(TblBookGround booking)
        {
            TblBookGround booked = _dbContext.TblBookGrounds.Where(x => x.GroundId == booking.GroundId && x.SessionId==booking.SessionId && x.Date==booking.Date).FirstOrDefault()!;
            if (booked is null)
            {
                return false;
            }
            return true;
        }

    }
}
