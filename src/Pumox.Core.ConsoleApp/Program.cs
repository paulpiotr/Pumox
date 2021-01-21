using System;

namespace Pumox.Core.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //new Pumox.Core.Database.Data.PumoxCoreDatabaseContext().CheckAndMigrate();
            //var connectionString = new Pumox.Core.Database.Models.AppSettings().GetConnectionString();
            //Console.WriteLine(connectionString);
        }
    }
}
