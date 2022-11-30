﻿using BAL.Abstraction;
using BAL.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DbModels;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroundController : BaseController
    {
        private readonly IGround _GroundService;

        public GroundController(DbSportsBuzzContext dbcontext,IGround GroundService) : base(dbcontext)
        {
            _GroundService = GroundService;
        }

        [HttpGet("GetGroundDetails")]
        [Authorize]
        public JsonResult GetGroundDetails()
        {
            try
            {
                return new JsonResult(_GroundService.GetGroundDetails().ToList());
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
                return new JsonResult(_GroundService.SearchByGroundCity(City).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost("GroundName")]
        [Authorize]
        public JsonResult SearchByGroundName(string GroundName)
        {
            try
            {
                return new JsonResult(_GroundService.SearchByGroundName(GroundName));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }

        [HttpPost]
        [Route("AddGround")]
        [Authorize(Policy = "Ground Manager")]
        public JsonResult AddGrounds(TblGround ground)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                _GroundService.AddGrounds(ground);
                crudStatus.Status = true;
                crudStatus.Message = "Ground added successfully";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        
        [HttpPut("EditeGround")]
        [Authorize(Policy = "Ground Manager")]
        public JsonResult EditGround(TblGround venu)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _GroundService.GroundChecking(venu);
                if (result == true)
                {
                    _GroundService.EditGround(venu);
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

        [HttpDelete("DeleteGroundDetails")]
        [Authorize(Policy = "Ground Manager")]
        public JsonResult DeleteGroundDetails(TblGround ground)
        {
            CrudStatus crudStatus = new CrudStatus();
            try
            {
                bool result = _GroundService.GroundChecking(ground);
                if (result == true)
                {
                    _GroundService.DeleteGroundDetails(ground);
                    crudStatus.Status = true;
                    crudStatus.Message = "Ground details deleted successfully";
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Ground not extist";
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
    
