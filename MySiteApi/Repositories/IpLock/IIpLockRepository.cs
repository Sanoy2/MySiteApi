using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MySiteApi.Repositories.IpLock
{
    public interface IIpLockRepository
    {
        Task AddAsync(IPAddress ip);
        Task<IEnumerable<IPAddress>> GetLockedIpsAsync();
        Task<bool> IsLockedAsync(IPAddress ip);
        bool IsLocked(IPAddress ip);
    }
}
