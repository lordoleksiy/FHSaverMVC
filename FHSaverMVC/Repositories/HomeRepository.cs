using FHSaverMVC.Context;
using FHSaverMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FHSaverMVC.Repositories
{
    public class HomeRepository: IHomeRepository
    {
        private readonly DBContext _dbContext;
        public HomeRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Folder> GetByIdAsync(long Id)
        {
            var folder = await _dbContext.Folders
               .Include(a => a.Children)
               .AsNoTracking()
               .SingleOrDefaultAsync(a => a.Id == Id);

            if (folder == null)
            {
                throw new CustomException(400, "No folder with such id!");
            }
            return folder;
        }

        public async Task<IEnumerable<Folder>> GetRootFoldersAsync()
        {
            var folders = _dbContext.Folders.Where(a => a.ParentFolder == null).AsNoTracking();
            return await folders.ToListAsync();
        }

        public async Task WriteFileAsync(IFormFile file)
        {
            using var streamReader = new StreamReader(file.OpenReadStream());

            var fileContent = await streamReader.ReadToEndAsync();
            var files = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

            await SetFileStructure(files, 0, null);
            _dbContext.SaveChanges();
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
                await SetFileStructure(folders, spaces + 1, parentFolder);
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

        public async Task<byte[]> GetAllAsync()
        {
            var rootFolders = await _dbContext.Folders.Include(x => x.Children).Where(x => x.ParentFolderId == null).AsNoTracking().ToListAsync();
            var text = await WriteFoldersToStringAsync(rootFolders, "", 0);
            return Encoding.UTF8.GetBytes(text);
        }

        private async Task<string> WriteFoldersToStringAsync(IEnumerable<Folder> folders, string text, int spaces)
        {
            if (folders == null)
            {
                return text;
            }
            
            
            foreach (var folder in folders) 
            {
                var subfold = await _dbContext.Folders.Include(x => x.Children).SingleAsync(a => a.Id == folder.Id);
                text += await WriteFoldersToStringAsync(subfold.Children, "".PadLeft(spaces) + folder.Name + "\n", spaces + 1);
            }
            return text;
        }

        public async Task<byte[]> GetFileById(long Id)
        {
            var folder = await _dbContext.Folders.Include(x => x.Children).SingleOrDefaultAsync(x => x.Id == Id);
            if (folder == null)
            {
                throw new CustomException(400, "No such id!");
            }
            var text = await WriteFoldersToStringAsync(folder.Children, $"{folder.Name}\n", 1);
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
