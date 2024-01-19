using BusinessLayer.Interface;
using CommonLayer.Model;
using DemoFundoo.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;

namespace DemoFundoo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        public readonly ICollabBusiness _Collab;
        public readonly ILoggerManager _logger;

        public CollabController(ICollabBusiness collab,ILoggerManager Logger)
        {
            this._Collab = collab;
            _logger = Logger;
        }

        [HttpPost]
        public IActionResult Collab([FromBody] CollabReq collab)
        {
            _logger.LogInfo("Here is info message from the controller.");
            try
            {
                collab.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                CollabEntity entity = _Collab.Collab(collab);
                return Ok(new {success = true,Messsage = "Collabrated",entity});
            }
            catch (Exception e)
            {

                return BadRequest(new { success = false, message = e.Message });   
            }
        }
        [HttpDelete]
        public IActionResult DeleteCollabs([FromBody]DeleteCollabReq collab)
        {
            try
            {
                collab.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                _Collab.DeleteCollabs(collab);
                return Ok(new { success = true, message = "Deleted the collab" });
            }
            catch (Exception e)
            {

                return BadRequest(new {success = false,message = e.Message});
            }
        }
        [HttpGet]
        public IActionResult GetAllCollabs()
        {
            try
            {
                long UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                List<CollabEntity> collabs = _Collab.GetAllCollabs(UserId);
                return Ok(new { success = true, message = "All the collabs", collabs });
            }
            catch (Exception e)
            {

                return BadRequest(new {success = false,message = e.Message});
            }
        }
    }
}
