using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IBookingGround
    {
        List<GroundList> GetGroundDetails(int userId, SearchAvailableGround availableGround);
        void BookingGround(TblBookGround booking);
        Notification GenerateMessage(TblBookGround booking);
        bool CheckExtistBookedDetails(TblBookGround booking);
    }
}
