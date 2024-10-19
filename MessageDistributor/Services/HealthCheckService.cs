using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MessageDistributor.Models;
using Serilog;

namespace MessageDistributor.Services
{
    public class HealthCheckService
    {
        private readonly HttpClient _httpClient;

        public HealthCheckService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendHealthCheckAsync()
        {
            // ایجاد پیام HealthCheck
            var healthCheck = new
            {
                Id = Guid.NewGuid().ToString(),
                SystemTime = DateTime.Now,
                NumberofConnectedClients = 5
            };

            var content = new StringContent(JsonSerializer.Serialize(healthCheck), Encoding.UTF8, "application/json");

            // ارسال درخواست HealthCheck به سامانه مدیریت
            bool success = false;
            int retryCount = 0;

            while (!success && retryCount < 5) // تلاش مجدد تا 5 بار در صورت شکست
            {
                var response = await _httpClient.PostAsync("http://localhost:7075/api/module/health", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var healthResponse = JsonSerializer.Deserialize<HealthCheckResponse>(responseContent);

                    if (healthResponse != null && healthResponse.IsEnabled)
                    {
                        Log.Information("HealthCheck successful. System is enabled.");
                        success = true;
                    }
                    else
                    {
                        Log.Warning("System is disabled, stopping message processing");
                        StopMessageProcessing();
                        break;
                    }
                }
                else
                {
                    retryCount++;
                    Log.Warning("HealthCheck failed with status code: {StatusCode}. Retrying... {RetryCount}/5", response.StatusCode, retryCount);
                    await Task.Delay(10000); // تأخیر 10 ثانیه‌ای قبل از تلاش مجدد
                }
            }

            if (!success)
            {
                Log.Error("HealthCheck failed after 5 attempts. Stopping the service.");
                StopService(); // متوقف کردن سرویس
            }
        }

        private void StopMessageProcessing()
        {
            // منطق برای متوقف کردن پردازش پیام‌ها
            Log.Information("Message processing has been stopped.");
        }

        private void StopService()
        {
            // منطق برای متوقف کردن کل سرویس
            Log.Information("Service has been stopped.");
        }
    }
}
