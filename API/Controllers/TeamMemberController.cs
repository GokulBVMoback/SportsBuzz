using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize(Policy = "Team Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : BaseController
    {
        private readonly ITeamMember _teamMemberService;
        private readonly CrudStatus crudStatus;

        public TeamMemberController(DbSportsBuzzContext dbcontext, ITeamMember teamMemberService)  :base(dbcontext)
        {
            _teamMemberService = teamMemberService;
            crudStatus = new CrudStatus();  
        }

        [HttpGet]
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

        [HttpPost]
        [Route("AddTeamMember")]
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

        [HttpPut]
        [Route("EditTeamMember")]
        public JsonResult EditTeamMember(TblTeamMember Player)
        {            
            try
            {
                bool result = _teamMemberService.CheckExtistTeamMember(Player);
                if (result == true)
                {
                    _teamMemberService.EditTeamMember(Player);
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
            try
            {
                bool result = _teamMemberService.CheckExtistTeamMember(Player);
                if (result == true)
                {
                    _teamMemberService.DeleteTeamMember(Player);
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
