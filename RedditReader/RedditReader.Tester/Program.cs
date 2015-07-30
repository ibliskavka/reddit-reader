using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedditReader.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new RedditClient(30);
            reader.AddSource("nsx");
            reader.AddSource("acura");
            
            reader.Next().ContinueWith(x =>
            {
                foreach (var redditItem in x.Result)
                {
                    Console.WriteLine(redditItem.Title);
                }
            });

            Console.WriteLine("Please wait for async operation to complete. Press Enter to close console.");
            Console.ReadLine();
        }
    }
}
