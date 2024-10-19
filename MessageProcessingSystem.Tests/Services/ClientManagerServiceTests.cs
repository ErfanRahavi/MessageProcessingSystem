using MessageDistributor.Services;
using Xunit;

public class ClientManagerServiceTests
{
    [Fact]
    public void TryAddClient_ShouldAddClient_WhenSpaceIsAvailable()
    {
        // Arrange
        var clientManager = new ClientManagerService(5); 

        // Act
        var result = clientManager.TryAddClient();

        // Assert
        Assert.True(result);
        Assert.Equal(1, clientManager.GetActiveClients());
    }

    [Fact]
    public void TryAddClient_ShouldNotAddClient_WhenMaxClientsReached()
    {
        // Arrange
        var clientManager = new ClientManagerService(1); 
        clientManager.TryAddClient(); 

        // Act
        var result = clientManager.TryAddClient();

        // Assert
        Assert.False(result);
        Assert.Equal(1, clientManager.GetActiveClients()); 
    }

    [Fact]
    public void RemoveClient_ShouldRemoveClient_WhenClientsArePresent()
    {
        // Arrange
        var clientManager = new ClientManagerService(5);
        clientManager.TryAddClient();

        // Act
        clientManager.RemoveClient();

        // Assert
        Assert.Equal(0, clientManager.GetActiveClients()); 
    }

    [Fact]
    public void RemoveClient_ShouldNotGoNegative_WhenNoClientsPresent()
    {
        // Arrange
        var clientManager = new ClientManagerService(5);

        // Act
        clientManager.RemoveClient();

        // Assert
        Assert.Equal(0, clientManager.GetActiveClients()); 
    }
}
