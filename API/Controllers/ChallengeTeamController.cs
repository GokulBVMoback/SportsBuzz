using BAL.Abstraction;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeTeamController : BaseController
    {
        private readonly IChallengeTeam _challengeTeam;
        public ChallengeTeamController(DbSportsBuzzContext dbcontext, IChallengeTeam challengeTeam) : base(dbcontext)
        {
            _challengeTeam = challengeTeam;
        }
        [HttpPost("GetAvailableTeamDetails")]
        //[Authorize]
        public JsonResult SearchAvailableTeam(SearchAvailableGround availablTeam)
        {
            try
            {
                return new JsonResult(_challengeTeam.SearchAvailableTeam(availablTeam).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("Challenge Team")]
        public JsonResult ChallengingTeam(TblChallenge challenge)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _challengeTeam.ChallengingTeam(challenge);
                if (result == false)
                {
                    result = _challengeTeam.ChallengingTeam(challenge);
                    crudStatus.Status = true;
                    crudStatus.Message = "challenged send successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "allready having match";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
