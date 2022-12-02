using Microsoft.AspNetCore.Mvc;
using BAL.Abstraction;
using BAL.Services;
using Models.DbModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize(Policy = "Team Manager")]
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
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _teamService.CheckExtistUserId(team);
                if (result == false)
                {
                    result = _teamService.CheckExtistTeam(team);
                    if (result == false)
                    {
                        _teamService.TeamRegistration(team);
                        crudStatus.Status = true;
                        crudStatus.Message = "Team added successfully";
                    }
                    else
                    {
                        crudStatus.Status = false;
                        crudStatus.Message = "Team name is already extist";
                    }
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "This user already have a team";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut]
        public JsonResult EditTeam(TblTeam TeamName)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _teamService.CheckExtistTeam(TeamName);
                if (result == false)
                {
                    _teamService.EditTeam(TeamName);
                    crudStatus.Status = true;
                    crudStatus.Message = "Team name is updated successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "New team name is already extist";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPut("Changing_Active_Status")]
        public JsonResult ChangingActiveStatus(int teamID)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _teamService.ChangingActiveStatus(teamID);
                crudStatus.Status = true;
                crudStatus.Message = "Active status changed successfully";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
