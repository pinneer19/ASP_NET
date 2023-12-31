using Microsoft.EntityFrameworkCore;
using Web_153502_Logvinovich.API.Data;
using Web_153502_Logvinovich.API.Services;
using Web_153502_Logvinovich.Api.Data;
using Web_153502_Logvinovich.Api.Services.AuthorService;
using Web_153502_Logvinovich.Api.Services.BookService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorServiceApi, AuthorService>();
builder.Services.AddScoped<IBookServiceApi, BookService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration.GetSection("isUri").Value;
    opt.TokenValidationParameters.ValidateAudience = false;
    opt.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
});


var app = builder.Build();

await DbInitializer.SeedData(app);
// Configure the HTTP request pipeline.

app.UseCors(opt =>
{
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
//app.UseRouting();
//app.MapControllerRoute(
//    name: "pager1",
//    pattern: "api/Books/{author:string}",
//    defaults: new { controller = "Home", action = "Index" }
//);
app.Run();