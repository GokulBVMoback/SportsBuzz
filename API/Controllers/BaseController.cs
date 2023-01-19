using BAL.Abstraction;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly DbSportsBuzzContext dbContext;
        public const string SessionKey = "UserId";
        public const string SessionToken = "Token";

        public BaseController(DbSportsBuzzContext dbcontext)
        {
            dbContext = dbcontext;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public int?  LoginId(string sessionkey)
        {
           var test=  HttpContext.Session.GetInt32(sessionkey);
            return test;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string? GetToken(string token)
        {
            var test = HttpContext.Session.GetString(token);
            return test;
        }
    }
}