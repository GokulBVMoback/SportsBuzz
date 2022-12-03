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
        List<GroundList> GetGroundDetails(SearchAvailableGround availableGround);
        bool BookingGround(TblBookGround booking);
        bool CheckExtistBookedDetails(TblBookGround booking);

    }
}
