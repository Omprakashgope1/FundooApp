using CommonLayer.Model;
using MassTransit;
using MessageReceiver.Consumer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Encodings;
using Repository.Context;
using Repository.Entity;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Service
{
    public class UserRepo : IUserRepo
    {
        public readonly FundooContext demoContext;
        public readonly IConfiguration config;
        private readonly IBus _bus;
        public UserRepo(FundooContext demoContext, IConfiguration configuration, IBus _bus)
        {
            this.demoContext = demoContext;
            this.config = configuration;
            this._bus = _bus;
        }
        public UserEntity Register(User UserReq)
        {
            UserEntity user = demoContext.Users.FirstOrDefault(x => x.Email == UserReq.Email);
            if (user != null)
            {
                throw new Exception("User with this email already present");
            }
            UserEntity userEntity = new UserEntity();
            userEntity.FirstName = UserReq.FirstName;
            userEntity.LastName = UserReq.LastName;
            userEntity.Email = UserReq.Email;
            userEntity.DateOfBirth = UserReq.DateOfBirth;
            userEntity.Password = (UserReq.Password);
            userEntity.CreatedAt = DateTime.Now;
            demoContext.Users.Add(userEntity);//To add to the database
            demoContext.SaveChanges();//to finalize the changes
            if (userEntity != null)
            {
                return userEntity;
            }
            return null;

        }
        //public string HashPassword(string password) 
        //{
        //    if (string.IsNullOrEmpty(password)) return null;
        //    else
        //    {
        //        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        //        return BCrypt.Net.BCrypt.HashPassword(password, salt);
        //    }
        //}

        public string Login(string Email, string Password)
        {
            List<UserEntity> users = demoContext.Users.ToList();
            var ParticularUser = users.FirstOrDefault(x => x.Email == Email && x.Password == Password);
            if (ParticularUser == null)
            {
                throw new Exception("User does not exists");
            }   
            string Tokken = GenerateToken(Email, ParticularUser.id);
            SendTokKen(Tokken,Email);
            return Tokken;
        }
        private string GenerateToken(string Email, long UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
        new Claim("Email", Email),
        new Claim("UserId", UserId.ToString())
    };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public string UpdatePassword(UserUpdatePasswordRequest ChangeReq)
        {
            List<UserEntity> users = demoContext.Users.ToList();
            var ParticularUser = users.FirstOrDefault(x => x.Email == ChangeReq.Email && x.Password == ChangeReq.PreviousPassword);
            if (ParticularUser == null)
            {
                throw new Exception("user not found");
            }
            ParticularUser.Password = ChangeReq.NewPassword;
            demoContext.SaveChanges();

            return "Password changed from " + ChangeReq.PreviousPassword + " to " + ChangeReq.NewPassword;
        }
        public String ForgetPassword(UserForgetRequest ForgetPassword)
        {
            try
            {
                List<UserEntity> users = demoContext.Users.ToList();
                var particularUser = users.FirstOrDefault(x => x.Email == ForgetPassword.Email);
                if (particularUser == null)
                {
                    throw new Exception("User of given Email does not exists");
                }
                string ParticularTokken = GenerateToken(particularUser.Email, particularUser.id);
                CreateTokken(ParticularTokken, particularUser.Email);
                SendTokKen(ParticularTokken, particularUser.Email);
                return "Successful";
            } catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> CreateTokken(string tokken, string Email)
        {
            Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            var ticket = new SendTicket
            {
                Email = Email,Tokken = tokken
            };

            await endPoint.Send(ticket);
            return "done";
        }
        public async Task<string> SendTokKen(string token, string emailTo)
        {
            string fromAddress = "omgope123@gmail.com";
            string toAddress = emailTo;
            string subject = "Token";
            string body = "The token for forgotten password is " + token;

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("omgope123@gmail.com", "unvu ubah dhvn xbdk");
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage(fromAddress, toAddress, subject, body))
                {
                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                        return "Email sent successfully.";
                    }
                    catch (Exception ex)
                    {
                        return $"Error sending email: {ex.Message}";
                    }
                }
            }
        }

    public UserEntity ResetPassword(ResetPasswordReq ResetPassword,string Email)
        {
            List<UserEntity> users = demoContext.Users.ToList();
            UserEntity user = users.FirstOrDefault(x => x.Email == Email);
            user.Password = ResetPassword.NewPassword;
            demoContext.SaveChanges();
            return user;
        }
    }
}
