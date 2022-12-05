using BAL.Abstraction;
using Entities.Models;
using Models.DbModels;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BAL.Services
{
    public class NotificationService : INotification
    {
        private readonly DbSportsBuzzContext _dbContext;

        public NotificationService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool SendWhatsAppNotification(Notification notification)
        {
            if (notification != null)
            {
                var accountSid = "AC57211c48dd683854b8b75c685fd9c78d";
                var authToken = "852917dae7fd8e6d2233f3ac5980dc6b";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:+91"+ notification.PhoneNum));
                messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                messageOptions.Body = notification.Message;
                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendSMSNotification(Notification notification)
        {
            if (notification != null)
            { 
                var accountSid = "AC57211c48dd683854b8b75c685fd9c78d";
                var authToken = "852917dae7fd8e6d2233f3ac5980dc6b";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber("+91" + notification.PhoneNum));
                messageOptions.MessagingServiceSid = "MG90324f61398df3d155459e7c9d0a87f0";
                messageOptions.Body = notification.Message;
                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SendWhatsAppNotification(TblChallenge challengeteam)
        {
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == challengeteam.TeamId2).FirstOrDefault()!;
            TblSession session = _dbContext.TblSessions.Where(x => x.SessionId == challengeteam.SessionId).FirstOrDefault()!;
            TblGround grd = _dbContext.TblGrounds.Where(x => x.GroundId == challengeteam.ChallengeId).FirstOrDefault()!;
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == grd.UserId).FirstOrDefault()!;
            if (challengeteam != null)
            {
                string msg = "Hello " + team.TeamName + " challenged" + grd.Venue + " on " + challengeteam.Date + " at " + session.Session;
                var accountSid = "AC57211c48dd683854b8b75c685fd9c78d";
                var authToken = "852917dae7fd8e6d2233f3ac5980dc6b";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                new PhoneNumber("whatsapp:+91" + user.PhoneNum));
                messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
                messageOptions.Body = msg;
                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                return true; 
            }
            else
            {
                return false;
            }
        }

        public bool SendSMSNotification(TblChallenge tblBookGround)
        {
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == tblBookGround.TeamId).FirstOrDefault()!;
            TblSession session = _dbContext.TblSessions.Where(x => x.SessionId == tblBookGround.SessionId).FirstOrDefault()!;
            TblGround grd = _dbContext.TblGrounds.Where(x => x.GroundId == tblBookGround.GroundId).FirstOrDefault()!;
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == grd.UserId).FirstOrDefault()!;
            if (tblBookGround != null)
            {
                string msg = "Hello " + team.TeamName + " booked your ground " + grd.Venue + " on " + tblBookGround.Date + " at " + session.Session;
                var accountSid = "AC57211c48dd683854b8b75c685fd9c78d";
                var authToken = "852917dae7fd8e6d2233f3ac5980dc6b";
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber("+91" + user.PhoneNum));
                messageOptions.MessagingServiceSid = "MG90324f61398df3d155459e7c9d0a87f0";
                messageOptions.Body = msg;

                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
