using Grpc.Core;
using ProcessorRequest = MessageProcessing.Processor.ProcessMessageRequest; 
 using MessageProcessor.Services;
using Moq;
using Xunit; 

namespace MessageProcessingSystem.Tests.Services
{
    public class MessageProcessorServiceTests
    {
        [Fact]
        public async Task ProcessMessage_ShouldReturnValidResponse_WithCorrectAnalysis()
        {
            // Arrange
            var service = new MessageProcessorService();
            var request = new ProcessorRequest
            {
                Id = "123",
                Message = "test message",
                AnalysisRules = { { "Rule1", ".*test.*" } }
            };

            // Act
            var result = await service.ProcessMessage(request, new Mock<ServerCallContext>().Object);

            // Assert
            Assert.Equal("123", result.Id);
            Assert.Equal("RegexEngine", result.Engine);
            Assert.Equal(12, result.MessageLength);
            Assert.True(result.IsValid);
        }
    }
}
