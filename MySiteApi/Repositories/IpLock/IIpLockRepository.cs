using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteApi.Repositories.IpLock
{
    public interface IIpLockRepository
    {
        Task AddAsync(System.Net.IPAddress ip);
        Task<IEnumerable<System.Net.IPAddress>> GetLockedIpsAsync();
        Task<bool> IsLockedAsync(System.Net.IPAddress ip);
        bool IsLocked(System.Net.IPAddress ip);
    }
}
