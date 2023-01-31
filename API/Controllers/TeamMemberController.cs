using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using Repository;
using System.Dynamic;
using Twilio.Jwt.Taskrouter;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    ///<summary>
    ///Api consist of  Team controller classes  drive from the controllerBase
    /// </summary>
    
    public class TeamMemberController : BaseController
    {
        /// <summary>
        /// By the dependency injection we are calling all the methods 
        /// </summary>

        private readonly ITeamMember _teamMemberService;
        private readonly CrudStatus crudStatus;

        /// <summary>
        /// In this constructor we are passing these parameters
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="teamMemberService"></param>
        
        public TeamMemberController(DbSportsBuzzContext dbcontext, ITeamMember teamMemberService) : base(dbcontext)
        {
            _teamMemberService = teamMemberService;
            crudStatus = new CrudStatus();
        }

        /// <summary>
        /// calling GetTeamMember() method from the TeamMemberService
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public JsonResult PlayerList()
        {
            try
            {
                return new JsonResult(_teamMemberService.GetTeamMember().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }


        [HttpGet]
        [Route("MyTeamMembers")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult MyTeamMembers(int teamID)
        {
            try
            {
                return new JsonResult(_teamMemberService.MyTeamMembers(teamID));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet]
        [Route("TeamMembersById")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult TeamMembersById(int playerID)
        {
            try
            {
                return new JsonResult(_teamMemberService.GetTeamMemberbyId(playerID));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        /// <summary>
        /// calling the AddTeamMember() from the TeamMemberService
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTeamMember")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult AddTeamMember(PlayerRegister player)
        {            
            try
            {
                var playerdto = AutoMapper<PlayerRegister, TblTeamMember>.MapClass(player);
                _teamMemberService.AddTeamMember(playerdto);
                crudStatus.Status = true;
                crudStatus.Message = "Player added successfully";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }            
        }

        /// <summary>
        /// calling EditTeamMember() method from the TeamMemberService
        /// </summary>
        /// <param name="Player"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("EditTeamMember")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult EditTeamMember(EditPlayer player)
        {            
            try
            {
                var playerdto = AutoMapper<EditPlayer, TblTeamMember>.MapClass(player);
                bool result = _teamMemberService.CheckExtistTeamMember(playerdto.MemberId);
                if (result == true)
                {
                    _teamMemberService.EditTeamMember(playerdto);
                    crudStatus.Status = true;
                    crudStatus.Message = "Player is updated successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Player is not extist";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        /// <summary>
        /// calling DeleteTeamMember() method from the TeamMemberService
        /// </summary>
        /// <param name="Player"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [Authorize(Policy = "Team Manager")]
        public JsonResult DeleteTeamMember(int memberID)
        {            
            try
            {
                bool result = _teamMemberService.CheckExtistTeamMember(memberID);
                if (result == true)
                {
                    _teamMemberService.DeleteTeamMember(memberID);
                    crudStatus.Status = true;
                    crudStatus.Message = "Player deleted successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Player not extist";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }
    }
}
