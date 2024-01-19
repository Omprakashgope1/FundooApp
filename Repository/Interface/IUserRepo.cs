using CommonLayer.Model;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUserRepo
    {
        public UserEntity Register(User user);
        public string Login(string Email, string Password);
        public string UpdatePassword(UserUpdatePasswordRequest ChangeReq);
        public String ForgetPassword(UserForgetRequest ForgetPassword);
        public UserEntity ResetPassword(ResetPasswordReq ResetPassword,string Email);
    }
}
