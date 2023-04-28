using Azure.Core.GeoJson;
using FHSaverMVC.Context;
using FHSaverMVC.Infrastructure;
using FHSaverMVC.Models;
using FHSaverMVC.Repositories;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FHSaverMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeRepository _repository;

        public HomeController(IHomeRepository repository)
        {
            _repository = repository;
        }

        public async ValueTask<IActionResult> IndexAsync(long? Id)
        {
            if (Id == null)
            {
                var folders = await _repository.GetRootFoldersAsync();
                return View("Index", folders);
            }
            else
            {
                var folder = await _repository.GetByIdAsync(Id.Value);
                return View("SubFolder", folder);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var file = HttpContext.Request.Form.Files["file"];
            if (file == null || file.Length == 0)
            {
                throw new CustomException(400, "File not found or empty");
            }
            if (file.ContentType != "text/plain")
            {
                throw new CustomException(400, "File types allows only txt flies");
            }

            await _repository.WriteFileAsync(file);
            return await IndexAsync(null);
        }

        public async Task<ActionResult> DownloadFile(string folderName)
        {
            if (folderName.IsNullOrEmpty())
            {
                throw new CustomException(400, "Id not found or empty");
            }

            byte[] data = null;
            if (folderName.Equals("All"))
            {
                data = await _repository.GetAllAsync();
            }
            return File(data, System.Net.Mime.MediaTypeNames.Text.Plain, folderName + ".txt");
        }
    }
}