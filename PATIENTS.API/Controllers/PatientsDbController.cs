using Microsoft.AspNetCore.Mvc;
using PATIENTS.APP.Domain;

namespace PATIENTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsDbController : ControllerBase
    {
        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var dbFactory = new PatientsDbFactory();

            dbFactory.SeedDb();
            
            return Ok("Database seed successful.");
        }
    }
}