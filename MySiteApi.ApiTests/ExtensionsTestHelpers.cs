using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MySiteApi.ApiTests
{
    public static class ExtensionsTestHelpers
    {
        public static IPAddress MakeIpAddress(this string address)
        {
            return IPAddress.Parse(address);
        }
    }
}
