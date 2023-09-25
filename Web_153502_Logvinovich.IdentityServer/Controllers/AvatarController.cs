using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Web_153502_Logvinovich.IdentityServer.Models;

namespace Web_153502_Logvinovich.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AvatarController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userMgr;
        
        public AvatarController(IWebHostEnvironment env, UserManager<ApplicationUser> userMgr)
        {
            _environment = env;
            _userMgr = userMgr;
        }

        [HttpGet]
        public IActionResult GetAvatar()
        {
            // Путь к папке, где хранятся аватары пользователей
            var avatarFolderPath = Path.Combine(_environment.ContentRootPath, "Images");

            // Получение имени файла аватара
            var id = _userMgr.GetUserId(User);
            var userAvatarFiles = Directory.GetFiles(avatarFolderPath, $"{id}.*");
            if(userAvatarFiles.Length > 0) {
                var avatarPath = userAvatarFiles[0];
                return GetAvatarFile(avatarPath);
            }
            else
            {
                var placeholderFilePath = Path.Combine(_environment.ContentRootPath, "Images", "avatar.png"); 
                return GetAvatarFile(placeholderFilePath);
                //var contentTypeProvider = new FileExtensionContentTypeProvider();
                //if (!contentTypeProvider.TryGetContentType("avatar.png", out var contentType))
                //{
                //    contentType = "application/octet-stream";
                //}

                //// Return the placeholder file
                //return File(fileBytes, contentType);
            }
        }

        private IActionResult GetAvatarFile(string path, bool useDefault = false)
        {
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            var fileExtension = Path.GetExtension(path);
            if (!contentTypeProvider.TryGetContentType(fileExtension, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            
            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, contentType);
        }

    }
}
