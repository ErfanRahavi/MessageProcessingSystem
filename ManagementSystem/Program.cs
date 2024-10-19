using Serilog;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. پیکربندی Serilog برای لاگ‌ها
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // ثبت لاگ‌ها در کنسول
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // ثبت لاگ‌ها در فایل
    .CreateLogger();

builder.Host.UseSerilog(); // استفاده از Serilog برای مدیریت لاگ‌ها

// 2. افزودن سرویس‌ها به DI container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // اضافه کردن Swagger برای مستندسازی API

// 3. ساخت برنامه
var app = builder.Build();

// 4. تنظیمات Middleware برای مدیریت درخواست‌ها
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // فعال‌سازی HTTPS

app.UseAuthorization(); // فعال‌سازی احراز هویت در صورت نیاز

app.MapControllers(); // استفاده از کنترلرها برای مدیریت درخواست‌ها

// 5. مدیریت خطاها
app.UseSerilogRequestLogging(); // استفاده از Serilog برای لاگ کردن درخواست‌ها

// 6. اجرای برنامه
app.Run();
