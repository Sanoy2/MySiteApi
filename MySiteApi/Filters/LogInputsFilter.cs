using Microsoft.AspNetCore.Mvc.Filters;
using MySiteApi.Others.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySiteApi.Filters
{
    public class LogInputsFilter : IActionFilter, IMyActionFilter
    {
        private readonly IMyLogger logger;

        public LogInputsFilter(IMyLogger logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Path      : {context.HttpContext.Request.Path}");
            builder.AppendLine($"Method    : {context.HttpContext.Request.Method}");
            builder.AppendLine($"Remote ip : {context.HttpContext.Connection.RemoteIpAddress.ToString()}");
            builder.AppendLine($"Local  ip : {context.HttpContext.Connection.LocalIpAddress.ToString()}");

            logger.Log(builder.ToString());
        }
    }
}
