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
using Repository;

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
builder.Services.AddScoped<IGenarate, Genarate_token>();
builder.Services.AddScoped<IBookingGround, BookingGroundService>();
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddScoped<IPagination, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
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
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1800);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
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
app.UseSession();
app.MapControllers();
app.Run();