using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize(Policy = "Team Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : BaseController
    {
        private readonly ITeamMember _TeamMemberService;
        
        public TeamMemberController(DbSportsBuzzContext dbcontext, ITeamMember TeamMemberService)  :base(dbcontext)
        {
            _TeamMemberService = TeamMemberService;
        }

        [HttpGet]
        public JsonResult PlayerList()
        {
            try
            {
                return new JsonResult(_TeamMemberService.GetTeamMember().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("AddTeamMember")]
        public JsonResult AddTeamMember(TblTeamMember Player)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                _TeamMemberService.AddTeamMember(Player);
                crudStatus.Status = true;
                crudStatus.Message = "Player added successfully";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }            
        }

        [HttpPut]
        [Route("EditTeamMember")]
        public JsonResult EditTeamMember(TblTeamMember Player)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _TeamMemberService.TeamMemberExtist(Player);
                if (result == true)
                {
                    _TeamMemberService.EditTeamMember(Player);
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

        [HttpDelete]
        [Route("delete")]
        public JsonResult DeleteTeamMember(TblTeamMember Player)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _TeamMemberService.TeamMemberExtist(Player);
                if (result == true)
                {
                    _TeamMemberService.DeleteTeamMember(Player);
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
