namespace SqliteCoreBox
{
    using System;
    using System.Diagnostics;
    using System.IO;

    class Program
    {
        static void Main()
        {
            File.Delete(Database.FileName);
            const int n = 100_000;
            using (var db = new Database())
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        db.Foos.Add(new Foo { Text = "abc" });
                    }

                    db.SaveChanges();
                    sw.Stop();
                    Console.WriteLine($"Inserting {n} items took {sw.ElapsedMilliseconds} ms ({sw.ElapsedMilliseconds / n} ms/insert), file size: {new FileInfo(Database.FileName).Length / (1024)} KB");

                    sw.Restart();
                    db.Foos.RemoveRange(db.Foos);
                    db.SaveChanges();
                    sw.Stop();
                    Console.WriteLine($"Removing {n} items took {sw.ElapsedMilliseconds} ms ({sw.ElapsedMilliseconds / n} ms/insert), file size: {new FileInfo(Database.FileName).Length / (1024)} KB");
                }
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
