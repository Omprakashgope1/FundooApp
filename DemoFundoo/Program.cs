using BusinessLayer.Interface;
using BusinessLayer.Services;
using DemoFundoo.Logger;
using MassTransit;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Repository.Context;
using Repository.Interface;
using Repository.Service;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static MassTransit.Transports.InMemory.Topology.Builders.PublishEndpointTopologyBuilder;

namespace DemoFundoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Console.WriteLine("Hello");
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<FundooContext>(opts =>
            opts.UseSqlServer(builder.Configuration["ConnectionString:DemoCreateDb"]));
            builder.Services.AddTransient<IUserBusiness, UserBusiness>();
            builder.Services.AddTransient<IUserRepo, UserRepo>();
            builder.Services.AddTransient<INoteRepo, NoteRepo>();
            builder.Services.AddTransient<INoteBussiness, NoteBusiness>();
            builder.Services.AddTransient<ICollabRepo, CollabRepo>();
            builder.Services.AddTransient<ICollabBusiness, CollabBusiness>(); 
            builder.Services.AddTransient<ILabelRepo,LabelRepo>();
            builder.Services.AddTransient<ILabelBusiness, LabelBusiness>();
            Console.WriteLine("Hello2");
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });
            Console.WriteLine("Hello3");

            builder.Services.AddMemoryCache();
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            Console.WriteLine("Hello4");

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
         builder => builder.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); 
            });
            Console.WriteLine("Hello5");

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Fundoo", Version = "v1" });
                //option.OperationFilter<SwaggerFileOperationFilter>();
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                }); ;
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            Console.WriteLine("Hello6");

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
            Console.WriteLine("Hello7");

            builder.Services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            Console.WriteLine("Hello8");

            builder.Services.AddMassTransitHostedService();
            var app = builder.Build();
            Console.WriteLine("Hello9");


            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FundooContext");
                });
            }
            Console.WriteLine("Hello10");

            app.Use(async (context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                    context.Response.StatusCode = 200;
                    await context.Response.CompleteAsync();
                }
                else
                {
                    await next();
                }
            });
            Console.WriteLine("Hello11");

            app.UseRouting();
            app.UseCors("AllowOrigin");
            Console.WriteLine("Hello13");

            app.UseAuthentication();
            Console.WriteLine("Hello14");

            app.UseAuthorization();
            Console.WriteLine("Hello15");




            app.MapControllers();

            Console.WriteLine("Hello16");

            app.Run();
            Console.WriteLine("Hello12");

        }

    }
}
