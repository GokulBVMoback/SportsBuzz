using BAL.Services;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using BAL.Abstraction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddNewtonsoftJson
    (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson
    (options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var connectionString = builder.Configuration.GetConnectionString("DBCon");
builder.Services.AddDbContext<DbSportsBuzzContext>(option =>
option.UseSqlServer(connectionString)
);
// Add services to the container.

builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IEncrypt, EncryptService>();
builder.Services.AddScoped<ITeam, TeamService>();
builder.Services.AddScoped<ITeamMember, TeamMemberService>();
builder.Services.AddScoped<IGround, GroundService>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ouath2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the bearer schema(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Ground Manager",
         policy => policy.RequireRole("Ground Manager"));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Team Manager",
         policy => policy.RequireRole("Team Manager"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
