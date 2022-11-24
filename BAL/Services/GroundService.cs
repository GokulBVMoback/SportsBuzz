using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class GroundService:IGround
    {
        private readonly DbSportsBuzzContext _dbContext;
        public GroundService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GroundList> GetTeamMember()
        {
            List<GroundList> result = (from ground in _dbContext.TblGrounds
                                       join user in _dbContext.TblUsers on ground.UserId equals user.UserId
                                       join sport in _dbContext.TblSportTypes on ground.SportType equals sport.SportTypeId
                                       orderby ground.GroundId
                                       select new GroundList
                                       {
                                           GroundId= ground.GroundId,
                                           FirstName= user.FirstName,
                                           LastName= user.LastName,
                                           CompanyName= ground.CompanyName,
                                           Venue= ground.Venue,
                                           City= ground.City,
                                           SportType=sport.SportType,
                                           CreatedDate= ground.CreatedDate,
                                           UpdatedDate= ground.UpdatedDate,
                                           Active= ground.Active
                                       }).ToList();
            return result.ToList();
        }

        public bool AddTeamMember(TblTeamMember Player)
        {
            Player.PlayerFirstName = Player.PlayerFirstName;
            Player.PlayerLastName = Player.PlayerLastName;
            Player.Age = Player.Age;
            Player.JerseyNo = Player.JerseyNo;
            _dbContext.TblTeamMembers.Add(Player);
            _dbContext.SaveChanges();
            return true;
        }

        public bool TeamMemberExtist(TblTeamMember Player)
        {
            TblTeamMember player = _dbContext.TblTeamMembers.Where(x => x.MemberId == Player.MemberId).FirstOrDefault()!;
            if (player is null)
            {
                return false;
            }
            return true;
        }

        public bool EditTeamMember(TblTeamMember Player)
        {
            TblTeamMember player = _dbContext.TblTeamMembers.Where(x => x.MemberId == Player.MemberId).FirstOrDefault()!;
            player.PlayerFirstName = player.PlayerFirstName;
            player.PlayerLastName = player.PlayerLastName;
            player.Age = player.Age;
            player.JerseyNo = player.JerseyNo;
            _dbContext.Entry(player).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteTeamMember(TblTeamMember Player)
        {
            TblTeamMember teamMember1 = _dbContext.TblTeamMembers.Where(x => x.MemberId == Player.MemberId).FirstOrDefault()!;
            _dbContext.TblTeamMembers.Remove(teamMember1);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
