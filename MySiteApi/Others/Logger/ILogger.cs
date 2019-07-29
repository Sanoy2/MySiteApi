using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteApi.Others.Logger
{
    public interface IMyLogger
    {
        void Log(string text);
        void Log(string label, string text);
        Task LogAsync(string text);
        Task LogAsync(string label, string text);
    }
}
