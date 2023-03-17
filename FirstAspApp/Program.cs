using FirstAspApp;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseMiddleware<VerifyProxyMiddleware>();

app.MapGet("/", (HttpContext context) => $"Hello World!\n {context.Items["proxy"]} \n{context.Items["data"]}");

app.Run();
