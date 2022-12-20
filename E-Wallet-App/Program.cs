using E_Wallet_App.Core.Core;
using E_Wallet_App.Core.Interface;
using E_Wallet_App.Core.Service;
using E_WalletApp.CORE.Core;
using E_WalletApp.CORE.Helper;
using E_WalletApp.CORE.Interface;
using E_WalletApp.CORE.Interface.RepoInterface;
using E_WalletApp.CORE.Service;
using E_WalletApp.DB.Context;
using E_WalletRepository.Repository;
using EmailService.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IValidation, Validation>();
builder.Services.AddScoped<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IWalletLogic, WalletLogic>();
builder.Services.AddScoped<ITransLogic, TransLogic>();
builder.Services.AddScoped<IESender, EmailSender>();
builder.Services.AddScoped<ITransService, TransService>();
builder.Services.AddDbContextPool<ApplicationContext>(options => options.UseSqlite("Data source =E-WalletDatabase"));
var emailconfig = builder.Configuration
   .GetSection("EmailCOnfiguration")
   .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailconfig);
//builder.Services.AddSingleton(builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
builder.Services.AddSwaggerGen(options => {

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer Scheme (\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options => {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            //ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
