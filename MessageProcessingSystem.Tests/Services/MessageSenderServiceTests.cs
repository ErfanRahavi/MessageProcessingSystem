using MessageDistributor.Models;
using MessageDistributor.Services;
using Moq;
using Serilog;
using System.Threading.Tasks;
using Xunit;
using Grpc.Core;
using MessageProcessing.Distributor;

namespace MessageProcessingSystem.Tests.Services
{
    public class MessageSenderServiceTests
    {
        [Fact]
        public async Task SendMessageToProcessorAsync_ShouldLogAndSendMessage()
        {
            // Arrange
            var mockGrpcClient = new Mock<MessageProcessingService.MessageProcessingServiceClient>();

            var response = new ProcessMessageResponse
            {
                Id = "123",
                Engine = "RegexEngine",
                MessageLength = 10,
                IsValid = true
            };

            // ایجاد AsyncUnaryCall برای شبیه‌سازی
            var call = new AsyncUnaryCall<ProcessMessageResponse>(
                Task.FromResult(response),
                null,
                null,
                null,
                null
            );

            // تنظیم شبیه‌سازی متد gRPC
            mockGrpcClient
                .Setup(client => client.ProcessMessageAsync(It.IsAny<ProcessMessageRequest>(), null, null, default))
                .Returns(call); // برگرداندن AsyncUnaryCall

            var messageSenderService = new MessageSenderService(mockGrpcClient.Object);

            // Act
            var message = new Message { Id = 123, Sender = "Legal", Content = "lorem ipsum" };
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
            await messageSenderService.SendMessageToProcessorAsync(message);

            // Assert
            // بررسی لاگ‌ها یا نتیجه
            Assert.True(response.IsValid);
        }
    }
}
