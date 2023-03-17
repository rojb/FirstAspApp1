
using Microsoft.Net.Http.Headers;

namespace FirstAspApp
{
    public class VerifyIpAddressMiddleware
    {
        private RequestDelegate _next;
        public VerifyIpAddressMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            string ipClient = context.Response.Headers["X-Client-IP"];
            Console.WriteLine(ipClient);
            string data = await GetIpApiData(ipClient);
            Console.WriteLine(data);
            await _next(context);
        }

        public async Task<string> GetIpApiData( string ipAddress)
        {
            string endPoint = $"http://ip-api.com/json/{ipAddress}?fields=status,message,country,countryCode,region,regionName,city,zip,lat,lon,timezone,isp,org,as,mobile,proxy,query";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(endPoint);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
