using Entities.Models;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Abstraction
{
    public interface IGround
    {
        List<GroundList> GetGroundDetails();
        bool AddGrounds(TblGround ground);
        bool GroundChecking(TblGround grounds);
        bool EditGround(TblGround ground);
        bool DeleteGroundDetails(TblGround ground);
        List<GroundList> SearchByGroundCity(string City);
        GroundList SearchByGroundName(string Ground);
    }
}
