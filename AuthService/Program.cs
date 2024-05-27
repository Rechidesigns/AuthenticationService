using AuthService.Core.Enums;
using AuthService.Data;  // Ensure this namespace is correct for your AppDbContext
using AuthService.Data.UserDatas.Model;
using AuthService.Helpers;
using AuthService.Services.UserManagement.Implementation;
using AuthService.Services.UserManagement.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register IUserService with its implementation UserService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<IEmailService, EmailClientService>();

// Register JwtConfig
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.AddSingleton(jwtConfig);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "1.0" });
    c.UseAllOfToExtendReferenceSchemas();
    c.UseAllOfForInheritance();
    c.UseOneOfForPolymorphism();
    c.SelectDiscriminatorNameUsing(type =>
    {
        return type.Name switch
        {
            nameof(ApplicationUser) => "Usertype",
            _ => null
        };


    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
    c.SchemaFilter<EnumSchemaFilter>();
    c.UseInlineDefinitionsForEnums();
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        throw new InvalidOperationException("Connection String is not found"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.MapInboundClaims = true;
    //var key = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtConfig.ValidIssuer,
        ValidAudience = jwtConfig.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();

app.MapControllers();

app.Run();










//using AuthService.Core.Enums;
//using AuthService.Data;  // Ensure this namespace is correct for your AppDbContext
//using AuthService.Data.UserDatas.Model;
//using AuthService.Helpers;
//using AuthService.Services.UserManagement.Implementation;
//using AuthService.Services.UserManagement.Interface;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();

//// Register IUserService with its implementation UserService
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IMailSender, MailSender>();
//builder.Services.AddScoped<IEmailService, EmailClientService>();


//// Register JwtConfig
//var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
//builder.Services.AddSingleton(jwtConfig);


//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "1.0" });
//    c.UseAllOfToExtendReferenceSchemas();
//    c.UseAllOfForInheritance();
//    c.UseOneOfForPolymorphism();
//    c.SelectDiscriminatorNameUsing(type =>
//    {
//        return type.Name switch
//        {
//            nameof(ApplicationUser) => "Usertype",
//            _ => null
//        };
//    });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Description = "JWT Authorization header \"Authorization: Bearer {token}\"",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.ApiKey
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
//        {
//            new OpenApiSecurityScheme{
//                Reference = new OpenApiReference{
//                    Id = "Bearer", // The name of the previously defined security scheme.
//                    Type = ReferenceType.SecurityScheme
//                },
//            },
//            new List<string>()
//        }
//    });
//    c.SchemaFilter<EnumSchemaFilter>();
//    c.UseInlineDefinitionsForEnums();
//});

//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
//        throw new InvalidOperationException("Connection String is not found"));
//});

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();

//// JWT 
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateIssuerSigningKey = true,
//        ValidateLifetime = true,
//        ValidIssuer = jwtConfig.ValidIssuer,
//        ValidAudience = jwtConfig.ValidAudience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
//    };
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthentication(); // Ensure authentication middleware is added
//app.UseAuthorization();

//app.MapControllers();

//app.Run();


