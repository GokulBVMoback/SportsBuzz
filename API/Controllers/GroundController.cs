using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using Repository;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroundController : BaseController
    {
        private readonly IGround _groundService;
        private readonly CrudStatus crudStatus;

        public GroundController(DbSportsBuzzContext dbcontext,IGround groundService) : base(dbcontext)
        {
            _groundService = groundService;
            crudStatus = new CrudStatus();
        }

        [HttpGet("GetGroundDetails")]
        [Authorize]
        public JsonResult GetGroundDetails()
        {
            try
            {
                return new JsonResult(_groundService.GetGroundDetails().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("Ground_City")]
        [Authorize]
        public JsonResult SearchByGroundCity(string City)
        {
            try
            {
                return new JsonResult(_groundService.SearchByGroundCity(City).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpGet("GroundName")]
        [Authorize]
        public JsonResult SearchByGroundName(string GroundName)
        {
            try
            {
                return new JsonResult(_groundService.SearchByGroundName(GroundName));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("AddGround")]
        [Authorize(Policy = "Ground Manager")]
        public JsonResult AddGrounds(GroundRegister ground)
        {
            try
            {
                var grounddto = AutoMapper<GroundRegister, TblGround>.MapClass(ground);
                _groundService.AddGrounds(grounddto);
                crudStatus.Status = true;
                crudStatus.Message = "Ground added successfully";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        
        [HttpPut("EditGround")]
        [Authorize(Policy = "Ground Manager")]
        public JsonResult EditGround(TblGround venu)
        {
            try
            {
                bool result = _groundService.GroundChecking(venu);
                if (result == true)
                {
                    _groundService.EditGround(venu);
                    crudStatus.Status = true;
                    crudStatus.Message = "Ground details are updated successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Ground does not exist";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPut("Changing_Active_Status")]
        [Authorize(Policy = "Ground Manager")]
        public JsonResult ChangingActiveStatus(int groundID)
        {
            try
            {
                _groundService.ChangingActiveStatus(groundID);
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
    
