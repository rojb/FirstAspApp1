using FirstAspApp;
using Microsoft.AspNetCore.HttpOverrides;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});
app.UseMiddleware<VerifyProxyMiddleware>();

app.MapGet("/", (HttpContext context) => $"Hello World!\n {context.Items["proxy"]} \n{context.Items["data"]}");

app.Run();
