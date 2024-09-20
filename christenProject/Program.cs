using christenProject.Authontication;
using christenProject.Data.Repo;
using christenProject.Data.Services;
using christenProject.Model;
using christenProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Data;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    options.SuppressModelStateInvalidFilter = true);
builder.Services.AddAutoMapper(typeof(Program)); // AutoMapper will scan for profiles                                     // Other service registrations..
builder.Services.AddDbContext<ITIContext>(option=>option.UseSqlServer(builder.Configuration.GetConnectionString("ITIDBCS")));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ITIContext>().AddDefaultTokenProviders();
ConfigureJWTBearer(builder, builder.Configuration);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CateogryService));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", profile =>
    {
        profile.AllowAnyHeader().AllowAnyMethod()
     .AllowAnyOrigin().Build();
    });
    options.AddPolicy("CorsPolicy2", profile =>
    {
        profile.AllowAnyHeader().AllowAnyMethod()
    .AllowCredentials().WithOrigins("http://127.0.0.1:3000", "https://localhost:7093");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
#region swaggerGenerate
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example:\"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
       {
            new OpenApiSecurityScheme{Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
       },   new string[] { }
    }
   });
});
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseStaticFiles();//for wwwroot folder files
app.UseCors("CorsPolicy2");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


static void ConfigureJWTBearer(IHostApplicationBuilder hostApplicationBuilder,IConfiguration configuration)
{
    hostApplicationBuilder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(jwtoptions =>
    {
        
        var secret = configuration["JWTConfig:Secret"];
        var issuer = configuration["JWTConfig:ValidIssuer"];
        var audience = configuration["JWTConfig:ValidAudiences"];
        if (secret is null || issuer is null || audience is null)
            throw new SecurityTokenExpiredException("Error in Credentials");
        jwtoptions.SaveToken = true;
        jwtoptions.RequireHttpsMetadata = false;
        jwtoptions.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
    });
}