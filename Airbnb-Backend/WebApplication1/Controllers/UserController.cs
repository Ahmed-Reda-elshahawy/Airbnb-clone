using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<ApplicationUser> _irepo;

        public UserController(IRepository<ApplicationUser> irepo)
        {
            _irepo = irepo;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ApplicationUser>> GetAll()
        {
            var users = _irepo.GetAll();
            return Ok(users);
        }

    }
}
