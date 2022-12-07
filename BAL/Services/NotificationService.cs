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
    }
}
