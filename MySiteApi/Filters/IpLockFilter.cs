using Microsoft.AspNetCore.Mvc.Filters;
using MySiteApi.Others.Logger;
using MySiteApi.Repositories.IpLock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySiteApi.Filters
{
    public class IpLockFilter : IActionFilter, IMyActionFilter
    {
        private readonly IIpLockRepository ipLockRepository;
        private readonly IMyLogger logger;

        public IpLockFilter(IIpLockRepository ipLockRepository, IMyLogger logger)
        {
            this.ipLockRepository = ipLockRepository;
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.Log("Everything fine");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var ip = context.HttpContext.Connection.RemoteIpAddress;
            if (ipLockRepository.IsLocked(ip))
            {
                logger.Log($"Illegal ip: {ip.ToString()}");
                throw new Exception();
            }
        }
    }
}
