using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System.ComponentModel;

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

        public List<BookedGroundView> BookingDetails()
        {
            List<BookedGroundView> list = _dbContext.BookedGroundViews.ToList();
            return list;
        }

        public List<BookedGroundView> MyBookingDetails(int? id)
        {
            List<TblTeam> teams= _dbContext.TblTeams.Where(x=>x.UserId== id).ToList();
            List<BookedGroundView> list = _dbContext.BookedGroundViews.ToList();
            foreach (var team in  teams)
            {
                BookedGroundView booked=list.Where(x=>x.UserId!=team.UserId).FirstOrDefault()!;
                list.Remove(booked);
            }
            return list;
        }

        public List<GroundList> GetGroundDetails(SearchAvailableGround availableGround)
        {
            List<TblBookGround> list = _dbContext.TblBookGrounds.ToList().Where(x => x.SessionId == availableGround.SessionId && x.Date==availableGround.Date).ToList();
            List<GroundList> grounds = ground.SearchByGroundCity(availableGround.City!).Where(x => x.SportType == availableGround.SportType && x.Active==true).ToList();            
            foreach (var items in list)
            {
                GroundList ground = grounds.Where(x => x.GroundId == items.GroundId).FirstOrDefault()!;
                grounds.Remove(ground);
            }            
            return grounds;
        }

        public void BookingGround(TblBookGround booking)
        {
            _dbContext.TblBookGrounds.Add(booking);
            _dbContext.SaveChanges();
            Notification notification = GenerateMessage(booking);
            _notification.SendWhatsAppNotification(notification);
            _notification.SendSMSNotification(notification);
        }

        public Notification GenerateMessage(TblBookGround booking)
        {
            Notification notification= new Notification();
            BookedGroundView details = _dbContext.BookedGroundViews.Where(x=>x.BookedId==booking.BookedId).FirstOrDefault()!;
            notification.Message= "Hello " + details.TeamName + " team booked your ground " + details.Venue + " on " +  String.Format($"{details.Date:dd-MM-yyyy}") + " at " + details.Session;
            notification.PhoneNum = details.PhoneNum.ToString();
            return notification;
        }

        public bool CheckExtistBookedDetails(TblBookGround booking)
        {
            TblBookGround booked = _dbContext.TblBookGrounds.Where(x => x.GroundId == booking.GroundId && x.SessionId==booking.SessionId && x.Date==booking.Date).FirstOrDefault()!;
            return booked!=null;
        }
    }
}
