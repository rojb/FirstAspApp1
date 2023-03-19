using Newtonsoft.Json.Linq;
using System.Net;
namespace FirstAspApp
{
    public class VerifyProxyMiddleware
    {
        private readonly RequestDelegate _next;
        public VerifyProxyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            const string FAIL = "fail";
            const string TRUE = "true";

            string ipClient = GetIpAddress(context);
            if (ipClient != "")
            {
                string data = await GetIpApiData(ipClient);
                JObject json = JObject.Parse(data);
                string requestStatus = json["status"]!.ToString();
                if (requestStatus != FAIL)
                {
                    string isProxy = json["proxy"]!.ToString();
                    if (isProxy == TRUE)
                    {
                        context.Items["proxy"] = "Proxy detected";
                        // TODO: Deny connection to the page
                    }


                }
                context.Items["proxy"] = "No proxy detected";
                context.Items["data"] = data;


            }
            await _next(context);
        }
        public string GetIpAddress(HttpContext context)
        {
          
            IPAddress remoteIpAddress = context.Response.HttpContext.Connection.RemoteIpAddress;
            string result = "";
            /*if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
            .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }

            }*/
                result = remoteIpAddress.ToString();
            return result;
        }
        public async Task<string> GetIpApiData(string ipAddress)
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
