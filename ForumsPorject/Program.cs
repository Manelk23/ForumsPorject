using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository;
using ForumsPorject.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ForumsProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("DbForums") ?? throw new InvalidOperationException("Connection string 'DbForums' not found.");

builder.Services.AddDbContext<DB_ForumsDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true
        };
    });

builder.Services.AddHttpContextAccessor();
// Register distributed memory cache
builder.Services.AddDistributedMemoryCache();

// Configure session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



builder.Services.AddScoped<ForumHub>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSignalR();

builder.Services.AddLogging();
// Register repositories and services
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ForumService>();
builder.Services.AddScoped<ForumRepository>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ThemeRepository>();
builder.Services.AddScoped<UtilisateurRepository>();
builder.Services.AddScoped<UtilisateurService>();
builder.Services.AddScoped<DescussionRepository>();
builder.Services.AddScoped<DescussionService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<AppRoleRepository>();
builder.Services.AddScoped<AppRoleService>();
builder.Services.AddScoped<UtilisateurRoleRepository>();
builder.Services.AddScoped<UtilisateurRoleService>();
builder.Services.AddScoped<AppRole>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ForumHub>();



builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseMvc();
app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
    name: "utilisateur",
    pattern: "{controller=Utilisateur}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "forum",
    pattern: "{controller=Forum}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "theme",
    pattern: "{controller=Theme}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "discussion",
    pattern: "{controller=Discussions}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "message",
    pattern: "{controller=Message}/{action=Index}/{id?}");


app.Run();
