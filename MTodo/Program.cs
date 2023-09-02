using Microsoft.AspNetCore.Authentication.Cookies;
using MTodo.Persistance;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddPostgresql(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(p=>{
    p.LoginPath=new PathString("/auth/login");
    p.LogoutPath=new PathString("/auth/logout");
    p.Cookie.SameSite=SameSiteMode.Strict;
    p.Cookie.HttpOnly=true;
    p.ExpireTimeSpan=new TimeSpan(0,20,1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

