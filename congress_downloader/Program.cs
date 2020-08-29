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
                c.RegisterCommand<DownloadCommand>();
            });

            return executor.Execute(args);
        }
    }
}