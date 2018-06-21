namespace SqliteCoreBox
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Microsoft.Data.Sqlite;

    internal static class SelectAllBenchmark
    {
        internal static void Run()
        {
            Console.WriteLine(nameof(SelectAllBenchmark));
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
                var foos = db.Foos.ToList();
                sw.Stop();
                Console.WriteLine($"Selecting {foos.Count} items took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                foos = db.Foos.ToList();
                sw.Stop();
                Console.WriteLine($"Selecting {foos.Count} items took {sw.ElapsedMilliseconds:N1} ms.");
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
                var foos = db.Foos.ToList();
                sw.Stop();
                Console.WriteLine($"Selecting {foos.Count} items took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                foos = db.Foos.ToList();
                sw.Stop();
                Console.WriteLine($"Selecting {foos.Count} items took {sw.ElapsedMilliseconds:N1} ms.");
            }
        }

        internal static void Ado()
        {
            Console.WriteLine("ADO");
            using (var db = new SqliteConnection(Database.ConnectionString))
            {
                var sw = Stopwatch.StartNew();
                db.Open();
                var foos = db.Select().ToList();
                sw.Stop();
                Console.WriteLine($"Selecting {foos.Count} items took {sw.ElapsedMilliseconds:N1} ms.");

                sw.Restart();
                foos = db.Select().ToList();
                sw.Stop();
                Console.WriteLine($"Selecting {foos.Count} items took {sw.ElapsedMilliseconds:N1} ms.");
            }
        }

        private static IEnumerable<Foo> Select(this SqliteConnection connection)
        {
            using (var command = new SqliteCommand("SELECT \"Id\", \"Text\" FROM \"Foos\"", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var id = reader.GetOrdinal("Id");
                    var text = reader.GetOrdinal("Text");
                    while (reader.Read())
                    {
                        yield return new Foo
                        {
                            Id = reader.GetInt32(id),
                            Text = reader.GetString(text)
                        };
                    }
                }
            }
        }
    }
}