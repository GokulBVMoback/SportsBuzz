using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;


namespace BAL.Services
{
    public class TeamService : ITeam
    {
        private readonly DbSportsBuzzContext _dbContext;
        public TeamService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TeamList> GetTeam()
        {
            List<TeamList> result = (from team in _dbContext.TblTeams
                                        join sport in _dbContext.TblSportTypes on team.SportType equals sport.SportTypeId
                                        join user in _dbContext.TblUsers on team.UserId equals user.UserId
                                        orderby team.TeamId
                                        select new TeamList
                                        {
                                            TeamId = team.TeamId,   
                                            TeamName= team.TeamName,
                                            City= team.City,
                                            SportType= sport.SportType,
                                            Email= team.Email,
                                            PhoneNum= team.PhoneNum,
                                            FirstName=user.FirstName,
                                            LastName=user.LastName,
                                            Active = team.Active
                                        }).ToList();
            return result.ToList();
        }

        public List<TeamList> MyTeams(int? id)
        {
            List<TeamList> result = (from team in _dbContext.TblTeams
                                     join sport in _dbContext.TblSportTypes on team.SportType equals sport.SportTypeId
                                     join user in _dbContext.TblUsers on team.UserId equals user.UserId
                                     orderby team.TeamId
                                     where user.UserId == id
                                     select new TeamList
                                     {
                                         TeamId = team.TeamId,
                                         TeamName = team.TeamName,
                                         City = team.City,
                                         SportType = sport.SportType,
                                         Email = team.Email,
                                         PhoneNum = team.PhoneNum,
                                         FirstName = user.FirstName,
                                         LastName = user.LastName,
                                         Active=team.Active
                                     }).ToList();
            return result.ToList();
        }

        public List<TeamList> SearchByCity(string City)
        {
            return GetTeam().Where(x => x.City!.ToLower() == City.ToLower()).ToList();
        }

        public TeamList SearchByTeamName(string Team)
        {
            return GetTeam().Where(x=>x.TeamName!.ToLower()==Team.ToLower()).FirstOrDefault()!;
        }

        public bool CheckExtistTeam(TblTeam team)
        {
            TblTeam team1 = _dbContext.TblTeams.Where(x => x.TeamName == team.TeamName).FirstOrDefault()!;
            return  team1 != null;  
        }

        public void TeamRegistration(TblTeam team)
        {
            team.CreatedDate = DateTime.Now;
            team.UpdatedDate = null;
            team.Active = true;
            _dbContext.TblTeams.Add(team);
            _dbContext.SaveChanges();
        }

        public void EditTeam(TblTeam TeamName)
        {
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == TeamName.TeamId).FirstOrDefault()!;
            team.TeamName = TeamName.TeamName;
            team.UpdatedDate = DateTime.Now;
            _dbContext.Entry(team).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void ChangingActiveStatus(int teamID)
        {
            TblTeam team = _dbContext.TblTeams.Where(x => x.TeamId == teamID).FirstOrDefault()!;
            team.Active=team.Active==true?false:true;   
            team.UpdatedDate = DateTime.Now;
            _dbContext.Entry(team).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
