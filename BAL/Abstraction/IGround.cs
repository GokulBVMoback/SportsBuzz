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
        List<GroundList> SearchByGroundCity(string City);
        GroundList SearchByGroundName(string Ground);
        bool AddGrounds(TblGround ground);
        bool GroundChecking(TblGround grounds);
        bool EditGround(TblGround ground);
        bool ChangingActiveStatus(int groundID);

    }
}
