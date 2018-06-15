namespace SqliteCoreBox
{
    using System;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Database())
            {
                db.Foos.Add(new Foo { Text = "abc" });
                db.SaveChanges();

                foreach (var foo in db.Foos.ToList())
                {
                    Console.WriteLine($"Id: {foo.Id}, Text: {foo.Text}");
                }
            }
        }
    }
}
