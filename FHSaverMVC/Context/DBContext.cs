using FHSaverMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FHSaverMVC.Context
{
    public class DBContext: DbContext
    {
        public DBContext(DbContextOptions options): base(options) { }
        public DbSet<Folder> Folders { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>()
                .HasMany(f => f.Children)
                .WithOne(f => f.ParentFolder)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Folder>().HasData(
                new Folder("Creating Digital Images") { Id = 1 },
                new Folder("Recources") { Id = 2, ParentFolderId = 1 },
                new Folder("Evidence") { Id = 3, ParentFolderId = 1 },
                new Folder("Graphic Products") { Id = 4, ParentFolderId = 1},
                new Folder("Primary Sources") { Id = 5, ParentFolderId = 2},
                new Folder("Secondary Sources") { Id = 6, ParentFolderId = 2},
                new Folder("Process") { Id = 7, ParentFolderId = 4 },
                new Folder("Final Product") { Id = 8, ParentFolderId = 4 }
                );
        }
    }
}
