using BAL.Abstraction;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Models.DbModels;

namespace BAL.Services
{
    public class GroundService:IGround
    {
        private readonly DbSportsBuzzContext _dbContext;

        public GroundService(DbSportsBuzzContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GroundList> GetGroundDetails()
        {
            List<GroundList> result = (from ground in _dbContext.TblGrounds
                                       join sport in _dbContext.TblSportTypes on ground.SportType equals sport.SportTypeId
                                       orderby ground.GroundId
                                       select new GroundList
                                       {
                                           GroundId = ground.GroundId,
                                           CompanyName = ground.CompanyName,
                                           Venue = ground.Venue,
                                           City = ground.City,
                                           SportType = sport.SportType,
                                           Latitude= ground.Latitude,
                                           Longitude= ground.Longitude,
                                           Active= ground.Active
                                       }).ToList();
            return result.ToList();
        }

        public List<GroundList> MyGround(int? id)
        {
            List<GroundList> result = (from ground in _dbContext.TblGrounds
                                       join sport in _dbContext.TblSportTypes on ground.SportType equals sport.SportTypeId
                                       join user in _dbContext.TblUsers on ground.UserId equals user.UserId
                                       orderby ground.GroundId
                                       where user.UserId == id
                                       select new GroundList
                                       {
                                           GroundId = ground.GroundId,
                                           CompanyName = ground.CompanyName,
                                           Venue = ground.Venue,
                                           City = ground.City,
                                           SportType = sport.SportType,
                                           Latitude = ground.Latitude,
                                           Longitude = ground.Longitude,
                                           Active = ground.Active
                                       }).ToList();
            return result.ToList();
        }

        public List<GroundList> SearchByGroundCity(string City)
        {
            return GetGroundDetails().Where(x => x.City!.ToLower() == City.ToLower()).ToList();
        }

        public GroundList SearchByGroundName(string Ground)
        {
            return GetGroundDetails().Where(x => x.Venue!.ToLower() == Ground.ToLower()).FirstOrDefault()!;
        }

        public void AddGrounds(TblGround ground)
        {
            ground.CreatedDate = DateTime.Now;
            ground.UpdatedDate = null;
            ground.Active = true;
            _dbContext.TblGrounds.Add(ground);
            _dbContext.SaveChanges();
        }

        public bool GroundChecking(TblGround grounds)
        {
            TblGround AlreadyExsistGround = _dbContext.TblGrounds.Where(x => x.GroundId == grounds.GroundId).FirstOrDefault()!;
            return AlreadyExsistGround != null;
        }

        public void EditGround(TblGround ground)
        {
            TblGround ground1 = _dbContext.TblGrounds.Where(x => x.GroundId == ground.GroundId).FirstOrDefault()!;
            ground1.CompanyName = ground.CompanyName;
            ground1.Venue = ground.Venue;
            ground1.UpdatedDate = DateTime.Now; 
            _dbContext.Entry(ground1).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void ChangingActiveStatus(int groundId)
        {
            TblGround ground = _dbContext.TblGrounds.Where(x => x.GroundId == groundId).FirstOrDefault()!;
            ground.Active=ground.Active == true ? false : true;
            ground.UpdatedDate = DateTime.Now;
            _dbContext.Entry(ground).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
