using Grpc.Core;
using MessageProcessing.Processor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessageProcessor.Services
{
    public class MessageProcessorService : MessageProcessingService.MessageProcessingServiceBase
    {
        public override async Task<ProcessMessageResponse> ProcessMessage(ProcessMessageRequest request, ServerCallContext context)
        {
            try
            {
                // ثبت پیام دریافتی در لاگ
                Log.Information("Received message with Id: {Id}, Content: {Message}", request.Id, request.Message);

                // تحلیل طول پیام
                var messageLength = request.Message.Length;

                // تحلیل پویا با Dictionary از regex
                var regexResults = new Dictionary<string, bool>();
                foreach (var rule in request.AnalysisRules)
                {
                    var regex = new Regex(rule.Value); // اجرای regex روی پیام
                    var isMatch = regex.IsMatch(request.Message);
                    regexResults.Add(rule.Key, isMatch); // ذخیره نتایج regex
                }

                // ساخت پاسخ برای پردازش
                var response = new ProcessMessageResponse
                {
                    Id = request.Id, 
                    Engine = "RegexEngine", 
                    MessageLength = messageLength, 
                    IsValid = regexResults.Values.Contains(true) 
                };

                // اضافه کردن نتایج regex به فیلد AdditionalFields به صورت کلید-مقدار
                foreach (var result in regexResults)
                {
                    response.AdditionalFields.Add(result.Key, result.Value);
                }

                // بازگرداندن پاسخ به سامانه تقسیم پیام
                return await Task.FromResult(response);
            }
            catch (Exception ex)
            {
                Log.Error("Error processing message: {Message}", ex.Message);

                var errorResponse = new ProcessMessageResponse
                {
                    Id = request.Id,
                    Engine = "ErrorEngine",
                    MessageLength = 0,
                    IsValid = false,
                    AdditionalFields = { { "Error", false } }
                };

                return await Task.FromResult(errorResponse);
            }
        }
    }
}
