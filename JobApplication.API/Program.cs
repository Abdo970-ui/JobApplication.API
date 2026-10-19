using JobApplication.Application.Interfaces;
using JobApplication.Application.Services;
using JobApplication.Infrastructure.Prestance;
using JobApplication.Infrastructure.Repositores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;

namespace JobApplication.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add DbContext with SQL Server provider

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register the JobRepository as a scoped service

            builder.Services.AddScoped<JobService>();
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<ApplicationService>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IRecruiterRepository, RecruiterRepository>();
            builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
            // Register Auth services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


            // Register MediatR
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(JobService).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            // Add JWT Authentication
            var jwtKey = builder.Configuration["Jwt:Key"];
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!))
                };
            });

            builder.Services.AddAuthorization();

            // Add Scalar API reference
            //builder.Services.AddOpenApi();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.MapOpenApi();
                //app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();   // لازم قبل UseAuthorization
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}