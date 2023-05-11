using AdaStore.Shared.Conts;
using AdaStore.Shared.Data;
using AdaStore.Shared.Enums;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtKey"))),
        ClockSkew = TimeSpan.Zero
    });

builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api blazor store", Version = "v1" });
    c.AddSecurityDefinition(Conts.Bearer, new OpenApiSecurityScheme 
    {
        Name = Conts.Authorization,
        Type = SecuritySchemeType.ApiKey,
        Scheme = Conts.Bearer,
        BearerFormat = Conts.JWT,
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = Conts.Bearer
                }
                
            },
            new string[]{ }
        }
    });
});

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Lockout.AllowedForNewUsers = false;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


var scope = app.Services.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

if (!await roleManager.RoleExistsAsync(Conts.Admin))
    await roleManager.CreateAsync(new IdentityRole<int>(Conts.Admin));

if (!await roleManager.RoleExistsAsync(Conts.Buyer))
    await roleManager.CreateAsync(new IdentityRole<int>(Conts.Buyer));

if (await userManager.FindByEmailAsync("admin@adastore.co") == null)
{
    var user = new User
    {
        UserName = "admin@adastore.co",
        Email = "admin@adastore.co",
        Name = "Admin",
        Address = string.Empty,
        PhoneNumber = string.Empty,
        Document = string.Empty,
        Profile = Profiles.Admin,
    };

    var result = await userManager.CreateAsync(user, "1234567");

    if (result.Succeeded)
        await userManager.AddToRoleAsync(user, Conts.Admin);
}

app.Run();
