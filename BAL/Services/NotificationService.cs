using BAL.Abstraction;
using Entities.Models;
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

        public bool SendWhatsAppNotification(TblBookGround tblBookGround)
        {
            //TblBookGround SendNotification=_dbContext.TblBookGrounds.Where(x=>x.GroundId==tblBookGround.BookedId).FirstOrDefault()!;
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == tblBookGround.TeamId).FirstOrDefault()!;
            TblSession session = _dbContext.TblSessions.Where(x => x.SessionId == tblBookGround.SessionId).FirstOrDefault()!;
            TblGround grd = _dbContext.TblGrounds.Where(x => x.GroundId == tblBookGround.GroundId).FirstOrDefault()!;
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == grd.UserId).FirstOrDefault()!;
            if (tblBookGround != null)
            {
                string msg = "Hello " + team.TeamName + " booked your ground " + grd.Venue + " on " + tblBookGround.Date + " at " + session.Session;
                var accountSid = "AC57211c48dd683854b8b75c685fd9c78d";
                var authToken = "bea2f8a0a11f16c360185e2e4002d4cb";
                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(
                new PhoneNumber(/*"whatsapp:+918825547037" */"whatsapp:" +user.PhoneNum));
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

        public bool SendSMSNotification(TblBookGround tblBookGround)
        {
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == tblBookGround.TeamId).FirstOrDefault()!;
            TblSession session = _dbContext.TblSessions.Where(x => x.SessionId == tblBookGround.SessionId).FirstOrDefault()!;
            TblGround grd = _dbContext.TblGrounds.Where(x => x.GroundId == tblBookGround.GroundId).FirstOrDefault()!;
            TblUser user = _dbContext.TblUsers.Where(x => x.UserId == grd.UserId).FirstOrDefault()!;
            string num= user.PhoneNum.ToString();
            if (tblBookGround != null)
            {
                string msg = "Hello " + team.TeamName + " booked your ground " + grd.Venue + " on " + tblBookGround.Date + " at " + session.Session;
                var accountSid = "AC57211c48dd683854b8b75c685fd9c78d";
                var authToken = "bea2f8a0a11f16c360185e2e4002d4cb";
                TwilioClient.Init(accountSid, authToken);

                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber(/*"+918825547037"*/ num ));
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
