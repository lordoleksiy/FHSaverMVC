using Azure.Core.GeoJson;
using FHSaverMVC.Context;
using FHSaverMVC.Infrastructure;
using FHSaverMVC.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace FHSaverMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _dbContext;

        public HomeController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async ValueTask<IActionResult> IndexAsync(long? Id)
        {
            if (Id == null)
            {
                var folders = await _dbContext.Folders.Where(a => a.ParentFolder == null).AsNoTracking().ToListAsync();
                return View("Index", folders);
            }
            else
            {
                var folder = await _dbContext.Folders
               .Include(a => a.Children)
               .AsNoTracking()
               .SingleOrDefaultAsync(a => a.Id == Id);
                
                if (folder == null) 
                {
                    throw new CustomException(400, "No folder with such id!");
                }

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

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = await streamReader.ReadToEndAsync();
                var files = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

                await SetFileStructure(files, 0, null);
                _dbContext.SaveChanges();
            }

            return await IndexAsync(null);
        }

        private async Task SetFileStructure(List<string> folders, int spaces, Folder? parentFolder)
        {

            if (!folders.Where(a => SpacesCount(a) >= spaces).Any())
            {
                return;
            }

            var curFolders = new List<string>();
            foreach (var folder in folders)
            {
                var curspaces = SpacesCount(folder);
                if (curspaces < spaces)
                {
                    break;
                }
                if (curspaces == spaces)
                {
                    curFolders.Add(folder);
                }
            }

            if (curFolders.IsNullOrEmpty())
            {
                await SetFileStructure(folders, spaces+1, parentFolder);
                return;
            }
            
            curFolders.ForEach(async name =>
            {
                var folder = new Folder(name.TrimStart());
                if (parentFolder != null)
                {
                    folder.ParentFolder = parentFolder;
                }
                
                _dbContext.Add(folder);
                folders.Remove(name);
                await SetFileStructure(folders, spaces + 1, folder);
            });

        }

        private static int SpacesCount(string item) => item.Length - item.TrimStart().Length;
        struct FileContainer
        {
            public int index;
            public string name;
            public Folder folder;
        }
    }
}