namespace FHSaverMVC.Context.Entities
{
    public class Folder
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public ICollection<Folder> Children { get; set; }
        public Folder(string Name)
        {
            this.Name = Name;
            Children = new List<Folder>();
        }
    }
}
