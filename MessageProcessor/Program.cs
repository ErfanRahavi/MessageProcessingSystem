using MessageProcessor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// تنظیم Serilog برای لاگ‌ها
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// اضافه کردن سرویس‌های gRPC
builder.Services.AddGrpc();

// تنظیم Kestrel برای پشتیبانی از HTTP/2
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, o =>
    {
        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        o.UseHttps(); // فعال‌سازی HTTPS
    });
});

// ساخت برنامه
var app = builder.Build();

// پیکربندی gRPC برای سرویس پردازش پیام
app.MapGrpcService<MessageProcessorService>();

app.Run();
