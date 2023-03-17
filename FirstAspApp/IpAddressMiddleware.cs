using System.Net;

namespace FirstAspApp
{
    public class IpAddressMiddleware
    {
        private readonly RequestDelegate _next;
        public IpAddressMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            IPAddress remoteIpAddress = context.Request.HttpContext.Connection.RemoteIpAddress;
            string result = "";
            if (remoteIpAddress != null)
            {
                // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                // This usually only happens when the browser is on the same machine as the server.
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                result = remoteIpAddress.ToString();

            }
            context.Response.Headers.Add("X-Client-IP", result);


            /* string ipAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
             context.Response.Headers.Add("X-Client-IP", ipAddress);
            */
            await _next(context);
        }
    }
}
