using FHSaverMVC.Context.Entities;
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
        }
    }
}
