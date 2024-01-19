using CommonLayer.Model;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        public UserEntity Register(User user);
        public string Login(string Email, string Password);
        public string UpdatePassword(UserUpdatePasswordRequest ChangeReq);
        public string ForgetPassword(UserForgetRequest ForgetPassword);
        public UserEntity ResetPassword(ResetPasswordReq resetPassword,string Email);
    }
}
