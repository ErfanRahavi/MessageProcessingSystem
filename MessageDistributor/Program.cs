using MessageDistributor.Services;
using MessageProcessing.Distributor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

try
{
    // پیکربندی Serilog برای لاگ‌ها
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    builder.Host.UseSerilog();

    // تنظیم gRPC Client برای ارتباط با سامانه پردازش پیام
    builder.Services.AddGrpcClient<MessageProcessingService.MessageProcessingServiceClient>(o =>
    {
        o.Address = new Uri("https://localhost:5001"); 
    });

    // تنظیمات HealthCheck و Worker Service
    builder.Services.AddHttpClient<HealthCheckService>();
    builder.Services.AddSingleton<MessageSenderService>();
    builder.Services.AddSingleton<ClientManagerService>(sp => new ClientManagerService(5)); // محدودیت کلاینت‌ها به 5
    builder.Services.AddHostedService<Worker>();

    // تنظیم Kestrel برای پشتیبانی از HTTP/2
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(5002, o => o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2);
    });

    // ساخت برنامه
    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    // لاگ کردن خطا در صورت بروز
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

// جلوگیری از بسته شدن خودکار کنسول
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
