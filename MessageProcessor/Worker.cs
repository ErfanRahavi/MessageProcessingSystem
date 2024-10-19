using MessageProcessor.Utilities; 
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // تولید Id بر اساس MAC Address
            var uniqueId = UniqueIdGenerator.GenerateIdFromMacAddress();

            // ثبت Id در لاگ
            Log.Information("Generated unique Id: {UniqueId}", uniqueId);

            // شبیه‌سازی کار انجام شده
            await Task.Delay(1000, stoppingToken); // تأخیر یک ثانیه‌ای
        }
    }
}
