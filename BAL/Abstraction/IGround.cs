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
        List<GroundList> MyGround(int? id);
        List<GroundList> SearchByGroundCity(string City);
        GroundList SearchByGroundName(string Ground);
        void AddGrounds(TblGround ground);
        bool GroundChecking(TblGround grounds);
        void EditGround(TblGround ground);
        void ChangingActiveStatus(int groundID);

    }
}
