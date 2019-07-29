using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MySiteApi.Repositories.IpLock
{
    public class InMemoryIpLock : IIpLockRepository
    {
        private static IEnumerable<IPAddress> lockedIps = new List<IPAddress>();

        public async Task AddAsync(IPAddress ip)
        {
            await Task.Run(() => lockedIps.Append(ip));
        }

        public async Task<IEnumerable<IPAddress>> GetLockedIpsAsync()
        {
            return await lockedIps.ToAsyncEnumerable().ToList();
        }

        public bool IsLocked(IPAddress ip)
        {
            return IsLockedAsync(ip).Result;
        }

        public async Task<bool> IsLockedAsync(IPAddress ip)
        {
            return await Task.FromResult(lockedIps.Any(n => n.Equals(ip)));
        }
    }
}
