using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Web_153502_Logvinovich.IdentityServer.Data;
using Web_153502_Logvinovich.IdentityServer.Models;

namespace Web_153502_Logvinovich.IdentityServer
{
    public class SeedData
    {
        public static async void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var adminRole = roleMgr.FindByNameAsync("Admin").Result;
                if (adminRole == null)
                {
                    adminRole = new IdentityRole("Admin");
                    var result = roleMgr.CreateAsync(adminRole).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("admin role created");
                }
                else
                {
                    Log.Debug("admin role already exists");
                }

                var userRole = roleMgr.FindByNameAsync("User").Result;
                if (userRole == null)
                {
                    userRole = new IdentityRole("User");
                    var result = roleMgr.CreateAsync(userRole).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("user role created");
                }
                else
                {
                    Log.Debug("user role already exists");
                }

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var alice = userMgr.FindByNameAsync("alice").Result;
                if (alice == null)
                {
                    alice = new ApplicationUser
                    {
                        UserName = "alice",
                        Email = "AliceSmith@email.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("alice created");
                }
                else
                {
                    Log.Debug("alice already exists");
                }

                var bob = userMgr.FindByNameAsync("bob").Result;
                if (bob == null)
                {
                    bob = new ApplicationUser
                    {
                        UserName = "bob",
                        Email = "BobSmith@email.com",
                        EmailConfirmed = true
                    };
                    var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(bob, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    Log.Debug("bob created");
                }
                else
                {
                    Log.Debug("bob already exists");
                }

                var user = userMgr.FindByNameAsync("user").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "user@gmail.com"
                    };
                    var result = await userMgr.CreateAsync(user, "userPassword1/");
                    if (result.Succeeded)
                    {
                        await userMgr.AddToRoleAsync(user, "User");
                    }

                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }
                var admin = userMgr.FindByNameAsync("admin").Result;              
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin@gmail.com",
                        
                    };

                    var result = userMgr.CreateAsync(admin, "adminPassword1/").Result;

                    if (result.Succeeded)
                    {
                        await userMgr.AddToRoleAsync(admin, "Admin");
                    }
                }
                
            }
        }
    }
}