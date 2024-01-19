using BusinessLayer.Interface;
using BusinessLayer.Services;
using CommonLayer.Model;
using DemoFundoo.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Repository.Entity;
using Repository.Service;

namespace DemoFundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserBusiness UserBusiness;
        public readonly ILoggerManager _logger;
        public UserController(IUserBusiness userBusiness,ILoggerManager logger)
        {
            this.UserBusiness = userBusiness;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(User user)
        {
            _logger.LogInfo("Here is info message from the controller.");
            try
            {
                var result = UserBusiness.Register(user);
                if (result != null)
                {
                    _logger.LogInfo("Job Done");
                    return this.Ok(new { success = true, message = "Registration successful", data = result });//smd format
                }
                else
                    return this.BadRequest(new { success = false, message = "Registration unsuccessful" });
            }
            catch (Exception e)
            {
                return this.BadRequest(new { success = false, message = e.Message });
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginReq req)
        {
            try
            {
                string message1 = UserBusiness.Login(req.Email, req.Password);
                return this.Ok(new { success = true, message = "login successful",data = message1 });
            }
            catch(Exception e) 
            {
                return this.BadRequest(new { success = false, message = e.Message});
            }
        }
        [HttpPut]
        [Route("UpdatePassword")]
        public IActionResult UpdatePassword(UserUpdatePasswordRequest ChangeReq)
        {
            try
            {
                string NewMessage = UserBusiness.UpdatePassword(ChangeReq);
                return this.Ok(new { success = true, message = NewMessage });
            }
            catch (Exception)
            {

                return this.BadRequest(new { success = false, mesage = "User with given EmailId and Password not found" });
            }
        }
        [HttpPut]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword(UserForgetRequest forgetRequest)
        {
            try
            {
                string NewMessage = UserBusiness.ForgetPassword(forgetRequest);
                return this.Ok(new { success = true,message = "Message sent",Tokken = NewMessage});
            }
            catch(Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordReq ResetReq)
        {
            try
            {
                string email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
                UserEntity userEntity = UserBusiness.ResetPassword(ResetReq,email);
                return this.Ok(new { success = true, message = $"Password changed to {ResetReq.NewPassword}", userEntity });
            }
            catch (Exception e)
            {

                return this.BadRequest(new {success = false,message = e.Message});
            }
        }
    }
}
