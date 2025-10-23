using AbacatePay.Models;
using Xunit;

namespace AbacatePay.Tests.Models;

public class AbacatePayConfigTests
{
    [Fact]
    public void DefaultValues_ShouldBeSetCorrectly()
    {
        // Act
        var config = new AbacatePayConfig();

        // Assert
        Assert.Equal(string.Empty, config.ApiKey);
        Assert.Equal("https://api.abacatepay.com", config.BaseUrl);
        Assert.Equal(30, config.TimeoutSeconds);
        Assert.False(config.Sandbox);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var config = new AbacatePayConfig();

        // Act
        config.ApiKey = "test-key";
        config.BaseUrl = "https://test.example.com";
        config.TimeoutSeconds = 60;
        config.Sandbox = true;

        // Assert
        Assert.Equal("test-key", config.ApiKey);
        Assert.Equal("https://test.example.com", config.BaseUrl);
        Assert.Equal(60, config.TimeoutSeconds);
        Assert.True(config.Sandbox);
    }
}
