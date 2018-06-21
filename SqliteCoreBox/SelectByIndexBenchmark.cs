namespace SqliteCoreBox
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Microsoft.Data.Sqlite;

    internal static class SelectByIndexBenchmark
    {
        internal static void Run()
        {
            Console.WriteLine(nameof(SelectByIndexBenchmark));
            if (File.Exists(Database.FileName))
            {
                File.Delete(Database.FileName);
            }

            using (var db = new Database())
            {
                db.AddRange(
                    new Foo { Text = "1" },
                    new Foo { Text = "2" },
                    new Foo { Text = "3" },
                    new Foo { Text = "4" });
                db.SaveChanges();
            }

            Ef();
            Ef();
            EfInMemory();
            Ado();
            Ado();
        }

        internal static void Ef()
        {
            Console.WriteLine("EF");
            using (var db = new Database())
            {
                var sw = Stopwatch.StartNew();
                var first = db.Foos.First(x => x.Text == "1");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");
                
                sw.Restart();
                first = db.Foos.First(x => x.Text == "2");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                first = db.Foos.First(x => x.Text == "3");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                first = db.Foos.First(x => x.Text == "4");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");
            }
        }

        internal static void EfInMemory()
        {
            Console.WriteLine("EF in memory");
            using (var db = new InMemory())
            {
                db.AddRange(
                    new Foo { Text = "1" },
                    new Foo { Text = "2" },
                    new Foo { Text = "3" },
                    new Foo { Text = "4" });
                db.SaveChanges();

                var sw = Stopwatch.StartNew();
                var first = db.Foos.First(x => x.Text == "1");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");
                
                sw.Restart();
                first = db.Foos.First(x => x.Text == "2");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                first = db.Foos.First(x => x.Text == "3");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                first = db.Foos.First(x => x.Text == "4");
                sw.Stop();
                Console.WriteLine($"Selecting item {first.Id} took {sw.ElapsedMilliseconds:N1} ms.");
            }
        }

        internal static void Ado()
        {
            Console.WriteLine("ADO");
            using (var db = new SqliteConnection(Database.ConnectionString))
            {
                var sw = Stopwatch.StartNew();
                db.Open();
                var match = db.Select(1);
                sw.Stop();
                Console.WriteLine($"Selecting item {match.Id} took {sw.ElapsedMilliseconds:N1} ms.");
                sw.Restart();

                match = db.Select(2);
                sw.Stop();
                Console.WriteLine($"Selecting item {match.Id} took {sw.ElapsedMilliseconds:N1} ms.");
                sw.Restart();

                match = db.Select(3);
                sw.Stop();
                Console.WriteLine($"Selecting item {match.Id} took {sw.ElapsedMilliseconds:N1} ms.");
                sw.Restart();

                match = db.Select(4);
                sw.Stop();
                Console.WriteLine($"Selecting item {match.Id} took {sw.ElapsedMilliseconds:N1} ms.");
                sw.Restart();
            }
        }

        private static Foo Select(this SqliteConnection connection, int id)
        {
            using (var command = new SqliteCommand("SELECT \"Id\", \"Text\" FROM \"Foos\" WHERE \"Id\" = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        return new Foo
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Text = reader.GetString(reader.GetOrdinal("Text"))
                        };
                    }
                }
            }

            throw new InvalidOperationException();
        }
    }
}