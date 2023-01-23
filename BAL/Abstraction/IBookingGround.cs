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
        List<BookedGroundView> BookingDetails();
        List<BookedGroundView> MyBookingDetails(int? id);
        List<GroundList> GetGroundDetails(SearchAvailableGround availableGround);
        void BookingGround(TblBookGround booking);
        Notification GenerateMessage(TblBookGround booking);
        bool CheckExtistBookedDetails(TblBookGround booking);
    }
}
