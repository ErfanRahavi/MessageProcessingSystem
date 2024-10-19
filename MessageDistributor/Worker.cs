using MessageDistributor.Models;
using MessageDistributor.Services;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    private readonly MessageSenderService _senderService;
    private readonly ClientManagerService _clientManager;

    public Worker(MessageSenderService senderService, ClientManagerService clientManager)
    {
        _senderService = senderService;
        _clientManager = clientManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_clientManager.TryAddClient())
            {
                try
                {
                    // تولید پیام تصادفی
                    var message = new Message
                    {
                        Id = new Random().Next(1, 1000),
                        Sender = "Legal",
                        Content = "lorem ipsum"
                    };

                    Log.Information("Generated message from queue: {Id}, {Sender}, {Content}", message.Id, message.Sender, message.Content);

                    // ارسال پیام به پردازشگر
                    await _senderService.SendMessageToProcessorAsync(message);

                    // تأخیر 200 میلی‌ثانیه
                    await Task.Delay(200, stoppingToken);
                }
                finally
                {
                    _clientManager.RemoveClient();
                }
            }
            else
            {
                Log.Information("Maximum number of clients reached, waiting...");
                await Task.Delay(1000, stoppingToken); // تأخیر یک ثانیه‌ای در صورت پر بودن ظرفیت
            }
        }
    }
}
