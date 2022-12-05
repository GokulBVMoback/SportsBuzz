using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface INotification
    {
        bool SendWhatsAppNotification(Notification notification);
        bool SendSMSNotification(Notification notification);
    }
}
