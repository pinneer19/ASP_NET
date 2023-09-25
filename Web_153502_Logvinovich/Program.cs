using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Web_153502_Logvinovich;
using Web_153502_Logvinovich.Data;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
var uriData = new UriData();
builder.Configuration.GetSection("UriData").Bind(uriData);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IAuthorService, ApiAuthorService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<IBookService, ApiBookService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));


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
        //options.Events.OnRedirectToIdentityProvider = redirectContext =>
        //{
        //    if (redirectContext.Request.Path.StartsWithSegments("/Admin"))
        //    {
        //        if (redirectContext.Response.StatusCode == (int)HttpStatusCode.OK)
        //        {
        //            redirectContext.ProtocolMessage.State = options.StateDataFormat.Protect(redirectContext.Properties);
        //        }
                
        //    }
        //    return Task.CompletedTask;
        //};

        // Получить Claims пользователя
        //options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });
//builder.Services.AddAuthorization
//    (options =>
//        {
//            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//        }
//    );
var app = builder.Build();

app.UseAuthentication();
app.MapRazorPages().RequireAuthorization();
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

app.UseAuthorization();
//app.MapControllerRoute(    
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//);
app.MapAreaControllerRoute(
    name: "AreaAdmin",
    areaName: "Admin",
    pattern: "Admin/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
