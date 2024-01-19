using BusinessLayer.Interface;
using CommonLayer.Model;
using Repository.Entity;
using Repository.Interface;
using Repository.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserBusiness:IUserBusiness
    {
        public readonly IUserRepo UserRepo;
        public UserBusiness(IUserRepo userRepo)
        {
            UserRepo = userRepo;
        }
        public UserEntity Register(User user)
        {
            try
            {
                return UserRepo.Register(user);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public string Login(string Email, string Password)
        {
            try
            {
                string message = UserRepo.Login(Email, Password);
                return message;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string UpdatePassword(UserUpdatePasswordRequest ChangeReq)
        {
            return UserRepo.UpdatePassword(ChangeReq);  
        }
        public string ForgetPassword(UserForgetRequest ForgetPassword)
        {
            return UserRepo.ForgetPassword(ForgetPassword);
        }
        public UserEntity ResetPassword(ResetPasswordReq resetPassword,string Email)
        {
            return UserRepo.ResetPassword(resetPassword,Email);
        }
    }
    
}
