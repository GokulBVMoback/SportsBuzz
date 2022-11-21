using Microsoft.AspNetCore.Mvc;
using BAL.Abstraction;
using BAL.Services;
using Models.DbModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : BaseController
    {
        private readonly ITeam _teamService;
        public TeamController(DbSportsBuzzContext dbcontext, ITeam teamService):base(dbcontext)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public JsonResult TeamList()
        {
            try
            {
                return new JsonResult(_teamService.GetTeam().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("City")]
        public JsonResult SearchByCity(string City)
        {
            try
            {
                return new JsonResult(_teamService.SearchByCity(City).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("Team")]
        public JsonResult SearchByTeamName(string Team)
        {
            try
            {
                return new JsonResult(_teamService.SearchByTeamName(Team));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("TeamRegistration")]
        public JsonResult TeamRegistration(TblTeam team)
        {
            try
            {
                bool result = _teamService.CheckExtistUserId(team);
                if (result == false)
                {
                    result = _teamService.CheckExtistTeam(team);
                    if (result == false)
                    {
                        result = _teamService.TeamRegistration(team);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Registration process done" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new CrudStatus() { Status = result, Message = "Team name is already extist" });
                    }
                }
                return new JsonResult(new CrudStatus() { Status = result, Message = "This user already have a team" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut]
        public JsonResult EditTeam(TblTeam TeamName, string NewTeamName)
        {
            try
            {
                bool result = _teamService.CheckExtistTeam(TeamName);
                if (result == true)
                {
                    result = _teamService.CheckExtistTeam(NewTeamName);
                    if (result == false)
                    {
                        result = _teamService.EditTeam(TeamName, NewTeamName);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Team name is updated successfully" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new CrudStatus() { Status = false, Message = "New team name is already extist" });
                    }
                }
                return new JsonResult(new CrudStatus() { Status = result, Message = "Team name not matched" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpDelete]
        public JsonResult DeleteTeam(TblTeam team)
        {
            try
            {
                _teamService.DeleteTeam(team);
                return new JsonResult(new CrudStatus() { Status = true, Message = "Team delete successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}
