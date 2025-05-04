using TaskManagementSystem.Common.Utility;
using TaskManagementSystem.Providers;
using Rotativa.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddStudentManagementServices(builder.Configuration);
builder.Services.AddMvc().AddRazorRuntimeCompilation();

#region Session

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SANRC.Session";
    options.IdleTimeout = TimeSpan.FromHours(8);//You can set Time
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ISessionManager, SessionManager>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

var webRootPath = app.Environment.WebRootPath;
var rotativaPath = Path.Combine(webRootPath, "extrafolder", "rotativa");
RotativaConfiguration.Setup(webRootPath, rotativaPath);
app.Run();
