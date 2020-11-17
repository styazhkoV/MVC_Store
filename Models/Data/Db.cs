using System.Data.Entity;

namespace MVC_Store.Models.Data
{

    /*public class Db : DbContext
    {
        public DbSet<PagesDTO> Pages { get; set; }

    }*/
    //Связь между сущностью и базой данных
    public class Db : DbContext
    {
        // дописать)
        public Db() : base("DefaultConnection")
        { }
        public DbSet<PagesDTO> Pages { get; set; }
    }
}