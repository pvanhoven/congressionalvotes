using System;
using Oakton;

namespace congress_downloader
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Starting...");

            var executor = CommandExecutor.For(c =>
            {
                c.RegisterCommands(typeof(Program).Assembly);
            });

            return executor.Execute(args);
        }
    }
}