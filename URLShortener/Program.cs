using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using URLShortener.BLL.Services.Implemantations;
using URLShortener.DAL.EF;
using URLShortener.DAL.Repositories.Implemantations;
using URLShortener.DAL.Repositories.Interfaces;
using URLShortener.BLL.Services.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using URLShortener.Data;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Core;
using Microsoft.AspNetCore.Authorization;
using URLShortener.Component;
using AutoMapper;
using URLShortener.BLL.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});


builder.Services.AddControllersWithViews();

// enable caching
builder.Services.AddMemoryCache();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'JewelryWorkshopDBContextConnection' not found.");
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var loggingSection = builder.Configuration.GetSection("Logging");
Console.WriteLine(appSettingsSection.Value);
Console.WriteLine(loggingSection.Value);
builder.Services.Configure<AppSettings>(appSettingsSection);

// get security key
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);


builder.Services.AddDbContext<UserDbContext>((options) => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<DbContext, URLShortenerDbContext>((options) => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IURLInfoRepository, URLInfoRepository>();
builder.Services.AddScoped<IURLInfoService, URLInfoService>();
builder.Services.AddScoped<IURLShortenerService, URLShortenerService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddMvc();

builder.Services.AddLogging(loggingBuilder =>
{
    //  loggingBuilder.AddApplicationInsights(aiKey);
    loggingBuilder.AddConfiguration(loggingSection);
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    //  loggingBuilder.AddSerilog();
    //  loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>
    //             (typeof(Program).FullName, LogLevel.Trace);

});


builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
           .AddJwtBearer(options =>
           {
               // optionally can make sure the user still exists in the db on each call
               /*options.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var user = userService.GetById(context.Principal.Identity.Name);
                       if (user == null)
                       {
                           // return unauthorized if user no longer exists
                           context.Fail("Unauthorized");
                       }
                       return Task.CompletedTask;
                   }
               };*/

               options.SaveToken = true;
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   //ValidAudience = "http://dotnetdetail.net",
                   // ValidIssuer = "http://dotnetdetail.net",
                   RequireExpirationTime = false,
                   ClockSkew = TimeSpan.FromMinutes(5),
               };
           });
// add authorization
builder.Services.AddAuthorization(options => { });

// handle authorization policies dynamically
/*builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();*/

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
});
SeedDb.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(
   options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
