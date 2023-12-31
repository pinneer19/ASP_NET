using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Serilog;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Web_153502_Logvinovich;
using Web_153502_Logvinovich.Data;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Middleware;
using Web_153502_Logvinovich.Services;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        var scopes = builder.Configuration.GetSection("InteractiveServiceSettings:Scopes").Get<IEnumerable<string>>();
        options.Scope.AddRange(scopes);
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });

var uriData = new UriData();
builder.Configuration.GetSection("UriData").Bind(uriData);


builder.Services.AddHttpClient<IAuthorService, ApiAuthorService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();


// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapAreaControllerRoute(
    name: "AreaAdmin",
    areaName: "Admin",
    pattern: "Admin/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages().RequireAuthorization();
app.UseSession();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseSerilogRequestLogging();

app.Run();
