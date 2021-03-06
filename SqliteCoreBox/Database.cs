﻿namespace SqliteCoreBox
{
    using System.IO;
    using Microsoft.EntityFrameworkCore;

    public class Database : DbContext
    {
        public static readonly string FileName = Path.GetFullPath("test.db");

        public static readonly string ConnectionString = $"Data Source={FileName}";
        public Database()
        {
            if (!File.Exists(FileName))
            {
                this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
        }

        public DbSet<Foo> Foos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite(ConnectionString);
        }
    }
}