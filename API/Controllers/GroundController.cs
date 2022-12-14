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
    ///<summary>
    ///Api consist Ground controller classes that drive from the controllerBase
    ///</summary> 
    public class GroundController : BaseController
    {
        /// <summary>
        /// By the dependency injection we are calling all the methods 
        /// </summary>
        
        private readonly IGround _groundService;
        private readonly CrudStatus crudStatus;

        /// <summary>
        /// In this constructor we are passing these parameters
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <param name="groundService"></param>
        public GroundController(DbSportsBuzzContext dbcontext,IGround groundService) : base(dbcontext)
        {
            _groundService = groundService;
            crudStatus = new CrudStatus();
        }

        /// <summary>
        /// calling GetGroundDetails() from the GroundService
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// calling SearchByGroundCity() method from the GroundService
        /// </summary>
        /// <param name="City"></param>
        /// <returns></returns>
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


        /// <summary>
        /// calling SearchByGroundName() method from the GroundService
        /// </summary>
        /// <param name="GroundName"></param>
        /// <returns></returns>

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

        /// <summary>
        /// calling  AddGrounds() from the GroundService
        /// </summary>
        /// <param name="ground"></param>
        /// <returns></returns>
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

        /// <summary>
        ///  calling EditGround() method from the GroundService
        /// </summary>
        /// <param name="venu"></param>
        /// <returns></returns>
        [HttpPut("EditeGround")]
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

        /// <summary>
        /// calling ChangingActiveStatus() method from the GroundService
        /// </summary>
        /// <param name="groundID"></param>
        /// <returns></returns>
        [HttpPut("Changing_Active_Status")]
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
    
