using AL.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService _studentservice;
        public StudentController(IStudentService studentservice) { this._studentservice = studentservice; }

        [HttpGet("SaveStudentAutomatically")]
        public void SaveStudentAutomatically()
        {
            try
            {
                _studentservice.SaveStudentAuto();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
