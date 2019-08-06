using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteApi.Others.Logger
{
    public class ConsoleLogger : IMyLogger
    {
        public void Log(string text)
        {
            Console.WriteLine("********************");
            Console.WriteLine(text);
            Console.WriteLine();
        }

        public void Log(string label, string text)
        {
            Console.WriteLine("********************");
            Console.WriteLine($"   **{label}**");
            Log(text);
        }

        public void Log(object someObject)
        {
            Log(someObject.ToString());
        }

        public async Task LogAsync(string text)
        {
            await Task.Run(() => Log(text));
        }

        public async Task LogAsync(string label, string text)
        {
            await Task.Run(() => Log(label, text));
        }

        public async Task LogAsync(object someObject)
        {
            await LogAsync(someObject.ToString());
        }
    }
}
