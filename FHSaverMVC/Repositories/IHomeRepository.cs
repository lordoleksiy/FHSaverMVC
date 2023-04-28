using FHSaverMVC.Models;

namespace FHSaverMVC.Repositories
{
    public interface IHomeRepository
    {
        public Task<IEnumerable<Folder>> GetRootFoldersAsync();
        public Task<Folder> GetByIdAsync(long Id);
        public Task WriteFileAsync(IFormFile file);
        public Task<byte[]> GetAllAsync();
        public Task<byte[]> GetFileById();
    }
}
