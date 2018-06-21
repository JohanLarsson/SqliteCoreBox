namespace SqliteCoreBox
{
    using System;

    class Program
    {
        static void Main()
        {
            SelectAllBenchmark.Run();
            Console.WriteLine();
            SelectByIndexBenchmark.Run();
        }
    }
}
