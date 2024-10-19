using MessageDistributor.Services;
using Moq;
using Moq.Protected;
using Serilog;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class HealthCheckServiceTests
{
    [Fact]
    public async Task SendHealthCheckAsync_ShouldLogSuccess_WhenHealthCheckIsSuccessful()
    {
        
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"IsEnabled\": true, \"NumberOfActiveClients\": 3, \"ExpirationTime\": \"2024-10-15T15:10:00\"}")
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var service = new HealthCheckService(httpClient);

        // Act
        await service.SendHealthCheckAsync();

        
    }
}
