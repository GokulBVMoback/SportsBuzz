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
        public BaseController(DbSportsBuzzContext dbcontext)
        {
            dbContext = dbcontext;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void  LoginId(string sessionkey)
        {
           var test=  HttpContext.Session.GetInt32(sessionkey);
            
        }
    }
}