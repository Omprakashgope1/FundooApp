using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Entity;

namespace DemoFundoo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        public readonly ILabelBusiness LabelBuss;
        public LabelController(ILabelBusiness labelBuss)
        {
            LabelBuss = labelBuss;
        }
        [HttpPost]
        public IActionResult AddLabel([FromBody]LabelReq req)
        {
            try
            {
                req.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                LabelEntity Label = LabelBuss.AddLabel(req);
                return Ok(new { success = true,message = "Label Added",data = Label});

            }
            catch (Exception e)
            {

                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpDelete]
        public IActionResult DeleteLabel(DeleteLabelReq DeleteLabel)
        {
            try
            {
                DeleteLabel.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                LabelBuss.DeleteLabel(DeleteLabel);
                return Ok(new {success = true,message = "Label Deleted"});
            }
            catch (Exception e)
            {

                return BadRequest(new {success = false,message = e.Message});   
            }
        }
        [HttpPost]
        [Route("Edit")]
        public IActionResult EditLabel(EditLableReq req)
        {
            try
            {
                req.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                LabelEntity label = LabelBuss.EditLabel(req);
                return Ok(new { success = true, message = "Name Edited", data = label });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                long UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                List<LabelEntity> labels = LabelBuss.GetAll(UserId);
                return Ok(new { success = true, message = "All Labels Retrieved", data = labels });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpPut]
        public IActionResult GetLabelByTitle([FromBody]LabelIdReq req) 
        {
            try
            {
                long UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                LabelEntity entity = LabelBuss.GetByTitle(req, UserId);
                return Ok(new { success = true, message = "Got the label", data = entity });
            }
            catch (Exception e)
            {
                return BadRequest(new {success = false,message = e.Message});   
            }
        }
    }
}
