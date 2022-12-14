using Microsoft.AspNetCore.Mvc;
using BAL.Abstraction;
using BAL.Services;
using Models.DbModels;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Repository;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize(Policy = "Team Manager")]
    [Route("api/[controller]")]
    [ApiController]
    ///<summary>
    ///Api consist Team controller classes that drive from the controllerBase
    ///</summary>  
    
    public class TeamController : BaseController
    {
        /// <summary>
        /// By the dependency injection we are calling all the methods 
        /// </summary>
       
        private readonly ITeam _teamService;
        private readonly CrudStatus crudStatus;
      
        /// <summary>
        /// In this constructor we are passing these parameters
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="teamService"></param>
        /// 
        public TeamController(DbSportsBuzzContext dbcontext, ITeam teamService):base(dbcontext)
        {
            _teamService = teamService;
            crudStatus = new CrudStatus();
        }

        /// <summary>
        /// calling GetTeam() method from the TeamService
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// calling SearchByCity() method from the TeamService
        /// </summary>
        /// <param name="City"></param>
        /// <returns></returns>
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

        /// <summary>
        /// calling SearchByTeamName() method from the TeamService
        /// </summary>
        /// <param name="Team"></param>
        /// <returns></returns>
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

        /// <summary>
        ///calling TeamRegistration() method from the TeamService
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TeamRegistration")]
        public JsonResult TeamRegistration(TeamRegister team)
        {           
            try
            {
                var teamdto = AutoMapper<TeamRegister, TblTeam>.MapClass(team);
                bool result = _teamService.CheckExtistUserId(teamdto);
                if (result == false)
                {
                    result = _teamService.CheckExtistTeam(teamdto);
                    if (result == false)
                    {
                        _teamService.TeamRegistration(teamdto);
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

        /// <summary>
        /// calling Edit method  from the TeamService
        /// </summary>
        /// <param name="TeamName"></param>
        /// <returns></returns>
        [HttpPut]
        public JsonResult EditTeam(EditTeam teamName)
        {
            try
            {
                var teamNamedto = AutoMapper<EditTeam, TblTeam>.MapClass(teamName);
                bool result = _teamService.CheckExtistTeam(teamNamedto);
                if (result == false)
                {
                    _teamService.EditTeam(teamNamedto);
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

        /// <summary>
        /// calling the ChangingActiveStatus() from the TeamService
        /// </summary>
        /// <param name="teamID"></param>
        /// <returns></returns>
        [HttpPut("Changing_Active_Status")]
        public JsonResult ChangingActiveStatus(int teamID)
        {            
            try
            {
                _teamService.ChangingActiveStatus(teamID);
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
