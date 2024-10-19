using ManagementSystem.Controllers;
using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingSystem.Tests.Controllers
{
    public class HealthControllerTests
    {
        [Fact]
        public void PostHealthCheck_ShouldReturnOkResult_WithValidResponse()
        {
            // Arrange
            var controller = new HealthController();
            var model = new HealthCheckModel
            {
                Id = "123",
                SystemTime = DateTime.Now,
                NumberofConnectedClients = 5
            };

            // Act
            var result = controller.PostHealthCheck(model) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = result.Value as HealthCheckResponse;
            Assert.True(response.IsEnabled);
            Assert.InRange(response.NumberOfActiveClients, 0, 5);
            Assert.True(response.ExpirationTime > DateTime.Now);
        }
    }
}
