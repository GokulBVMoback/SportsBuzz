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
        private readonly INotification _notification;
        private readonly IGround ground;
       
        public BookingGroundService(DbSportsBuzzContext dbContext,IGround grnd, INotification notification)
        {
            _dbContext = dbContext;
            ground = grnd;
            _notification = notification;
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
            Notification notification = GenerateMessage(booking);
            _notification.SendWhatsAppNotification(notification);
            _notification.SendSMSNotification(notification);
            return true;
        }

        public Notification GenerateMessage(TblBookGround booking)
        {
            Notification notification= new Notification();
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == booking.TeamId).FirstOrDefault()!;
            TblSession session = _dbContext.TblSessions.Where(x => x.SessionId == booking.SessionId).FirstOrDefault()!;
            TblGround grd = _dbContext.TblGrounds.Where(x => x.GroundId == booking.GroundId).FirstOrDefault()!;
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == grd.UserId).FirstOrDefault()!;
            notification.Message= "Hello " + team.TeamName + " team booked your ground " + grd.Venue + " on " + booking.Date + " at " + session.Session;
            notification.PhoneNum = user.PhoneNum.ToString();
            return notification;
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
