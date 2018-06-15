namespace SqliteCoreBox
{
    using Microsoft.EntityFrameworkCore;

    public class Database : DbContext
    {
        private static bool _created = false;
        public Database()
        {
            if (!_created)
            {
                _created = true;
                this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite(@"Data Source=test.db");
        }
 
        public DbSet<Foo> Foos { get; set; }
    }
}