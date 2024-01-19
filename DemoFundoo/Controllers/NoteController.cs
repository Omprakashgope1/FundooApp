using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using Repository.Entity;
using System.Text;

namespace DemoFundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        public readonly INoteBussiness _NoteBussiness;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        public NoteController(INoteBussiness noteBussiness, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _NoteBussiness = noteBussiness;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
        }
        [HttpPost]
        [Route("Add")]
        [Authorize]
        public IActionResult MakeNote([FromBody] Note note)
        {
            try
            {
                note.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value); 
                NoteEntity notes = _NoteBussiness.MakeNote(note);
                return Ok(new {success = true,message = "Added successfully",notes});
            }
            catch (Exception e)
            {

                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteNote(GetNoteReq req)
        {
            try
            {
                req.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);   
                _NoteBussiness.DeleteNote(req);
                return Ok(new
                {
                    success = true,
                    message = "Permanently deleted"
                });
            }catch(Exception e) 
            {
                return BadRequest(new { success = false, message = e.Message });
            }  
        }
        [HttpPut]
        [Route("Edit")]
        public IActionResult EditNote([FromBody]EditNoteReq note)
        {
            try
            {
                note.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                NoteEntity noteEntity = _NoteBussiness.EditNote(note);
                return Ok(new {success = true,message = "Edited",noteEntity});
            }
            catch (Exception e)
            {
                return BadRequest(new {success = false, message = e.Message});
            }
        }
        [HttpPut]
        [Route("Trash/{id}")]
        public IActionResult Trash(long id)
        {
            try
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                NoteEntity note = _NoteBussiness.Trash(id, (long)userId);
                return Ok(new { success = true, message = "Moved to Trash", data = note });
            }catch(Exception e)
            {
                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                Console.WriteLine("what");
                long id = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                //long id = 2;
                //var cacheKey = "ls";
                //string NoteList;
                //var ls = new List<NoteEntity>();
                //var redisCustomerList = await distributedCache.GetAsync(cacheKey);
                //if (redisCustomerList != null)
                //{
                //    NoteList = Encoding.UTF8.GetString(redisCustomerList);
                //    ls = JsonConvert.DeserializeObject<List<NoteEntity>>(NoteList);
                //    return Ok(new { success = true, message = "Got all", data = ls });
                //}
                //else
                //{
                   List<NoteEntity> ls = _NoteBussiness.GetAll(id);
                    //NoteList = JsonConvert.SerializeObject(ls);
                    //redisCustomerList = Encoding.UTF8.GetBytes(NoteList);
                    //var options = new DistributedCacheEntryOptions()
                    //    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    //    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    //await distributedCache.SetAsync(cacheKey, redisCustomerList, options);
                    return Ok(new { success = true, message = "Got all", data = ls });
                //}
            }
            catch (Exception e)
            {

                return BadRequest(new {success = false,message = e.Message});
            }
        }
        [HttpPut]
        [Route("SetColor")]
        public IActionResult SetColor(ColChangeReq req) 
        {
            try
            {
                req.UserId = (long)Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                NoteEntity note = _NoteBussiness.SetColor(req);
                return Ok(new { success = true, message = "Color changed", note });
            }
            catch (Exception e)
            {

                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpPost]
        [Route("SetArchieve")]
        public IActionResult SetArchieve(SetArchieveReq req) 
        {
            try
            {
                req.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                NoteEntity note = _NoteBussiness.SetArchieve(req);
                return Ok(new { success = true, message = "Archieved", data = note });
            }
            catch (Exception e)
            {

                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpPut]
        [Route("Pin")]
        public IActionResult PinNote(PinNoteReq PinNote)
        {
            try
            {
                PinNote.UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                NoteEntity note = _NoteBussiness.PinNote(PinNote);
                return Ok(new { success = true, message = "Note pinned", note });
            }
            catch (Exception e)
            {

                return BadRequest(new {success = false, message = e.Message});  
            }
        }
        [HttpPut]
        [Route("GetById")]
        public async Task<IActionResult> GetById(long id) 
        {
            try
            {
                long UserId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var cacheKey = "note";
                string NoteList;
                NoteEntity note;
                var redisCustomerList = await distributedCache.GetAsync(cacheKey);
                if (redisCustomerList != null)
                {
                    NoteList = Encoding.UTF8.GetString(redisCustomerList);
                    note = JsonConvert.DeserializeObject<NoteEntity>(NoteList);
                    return Ok(new { success = true, message = "Got all", note });
                }
                note = _NoteBussiness.GetById(id, UserId);
                NoteList = JsonConvert.SerializeObject(note);
                redisCustomerList = Encoding.UTF8.GetBytes(NoteList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCustomerList, options);
                return Ok(new { success = true, Note = note });
            }
            catch (Exception e)
            {

                return BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpPut]
        [Route("AddImage")]
        public IActionResult AddImage([FromForm]AddImage image)
        {
            try
            {
                long UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);  
                NoteEntity note = _NoteBussiness.ChangeImage(image, UserId);
                return Ok(new { success = true, message = "Photo Uploaded", data = note });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false,message = e.Message});  
            }
        }
        [HttpGet]
        [Route("GetByKeyword")]
        public IActionResult GetByKeyWord(string KeyWord)
        {
            try
            {
                long UserId = (long)Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                List<NoteEntity> list = _NoteBussiness.GetByKeyWord(KeyWord,UserId);
                return Ok(new { success = true, Message = "Got all having keyword", data = list });
            }
            catch (Exception e)
            {
                return BadRequest(new {success = false, message = e.Message});  
            }
        }
    }
}
