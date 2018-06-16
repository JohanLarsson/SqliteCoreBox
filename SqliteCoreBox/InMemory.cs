namespace SqliteCoreBox
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class InMemory : DbContext
    {
        private readonly SqliteConnection connection;

        public InMemory()
        {
            this.connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            this.Database.EnsureCreated();
        }

        public DbSet<Foo> Foos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite(connection);
        }

        public override void Dispose()
        {
            this.connection.Dispose();
            base.Dispose();
        }
    }
}