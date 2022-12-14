using Application.DAOInterfaces;
using Application.Logic;
using Data;
using Data.DAOs;
using Application.LogicInterfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMemberDAO, MemberDAO>();
builder.Services.AddScoped<ITeamDAO, TeamDAO>();
builder.Services.AddScoped<IProjectDAO, ProjectDAO>();
builder.Services.AddScoped<IMeetingDAO, MeetingDAO>();
builder.Services.AddScoped<ITaskDAO, TaskDAO>();
builder.Services.AddScoped<ILogBookDAO, LogBookDAO>();

builder.Services.AddScoped<IMemberLogic, MemberLogic>();
builder.Services.AddScoped<ITeamLogic, TeamLogic>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectLogic, ProjectLogic>();
builder.Services.AddScoped<IMeetingLogic, MeetingLogic>();
builder.Services.AddScoped<ITaskLogic, TaskLogic>();
builder.Services.AddScoped<ILogBookLogic, LogBookLogic>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
