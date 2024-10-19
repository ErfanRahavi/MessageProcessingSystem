using MessageDistributor.Models;
using Grpc.Net.Client;
using MessageProcessing.Distributor;
using Serilog;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MessageDistributor.Services
{
    public class MessageSenderService
    {
        private readonly MessageProcessingService.MessageProcessingServiceClient _grpcClient;

        public MessageSenderService(MessageProcessingService.MessageProcessingServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task SendMessageToProcessorAsync(Message message)
        {
            Log.Information("Sending message to processor: {Id}, {Sender}, {Content}", message.Id, message.Sender, message.Content);

            // ایجاد درخواست gRPC برای پردازش پیام
            var request = new ProcessMessageRequest
            {
                Id = message.Id.ToString(),  
                Message = message.Content
            };

            // افزودن قوانین تحلیل به دیکشنری AnalysisRules
            var analysisRules = new Dictionary<string, string>
            {
                { "ContainsLorem", "lorem" },  // افزودن قوانین regex برای بررسی وجود 'lorem'
                { "ContainsIpsum", "ipsum" }   // افزودن قوانین regex برای بررسی وجود 'ipsum'
            };
            foreach (var rule in analysisRules)
            {
                request.AnalysisRules.Add(rule.Key, rule.Value);  // اضافه کردن قوانین به درخواست
            }

            var response = await _grpcClient.ProcessMessageAsync(request);

            // لاگ کردن نتیجه پردازش
            Log.Information("Processed message: {Id}, Engine: RegexEngine, Length: {Length}, IsValid: {IsValid}",
                            message.Id, response.MessageLength, response.IsValid);
        }
    }
}
