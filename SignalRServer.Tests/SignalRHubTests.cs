using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using DiscountCodes.SignalRServer.Services;

namespace DiscountCodes.SignalRServer.Tests;

public class SignalRHubTests
{
    private readonly Mock<IDiscountCodeService> _mockService;
    private readonly SignalRHub _sut;

    public SignalRHubTests()
    {
        _mockService = new Mock<IDiscountCodeService>();
        _sut = new SignalRHub(_mockService.Object);
    }

    [Theory]
    [InlineData(-1, 7)]
    [InlineData(2001, 7)]
    public async Task GenerateCodes_ShouldReturnFalse_WhenNumberIsOutOfRange(int number, byte length)
    {
        // Act
        var result = await _sut.GenerateCodes(number, length);

        // Assert
        Assert.False(result);
        _mockService.Verify(s => s.GenerateCodesAsync(It.IsAny<int>(), It.IsAny<byte>()), Times.Never);
    }

    [Theory]
    [InlineData(0, 6)]
    [InlineData(0, 9)]
    public async Task GenerateCodes_ShouldReturnFalse_WhenLengthIsOutOfRange(int number, byte length)
    {
        // Act
        var result = await _sut.GenerateCodes(number, length);

        // Assert
        Assert.False(result);
        _mockService.Verify(s => s.GenerateCodesAsync(It.IsAny<int>(), It.IsAny<byte>()), Times.Never);
    }

    [Fact]
    public async Task GenerateCodes_ShouldInvokeService_WhenParametersAreValid()
    {
        // Arrange
        int number = 10;
        byte length = 7;
        _mockService.Setup(s => s.GenerateCodesAsync(number, length)).ReturnsAsync(true);

        // Act
        var result = await _sut.GenerateCodes(number, length);

        // Assert
        Assert.True(result);
        _mockService.Verify(s => s.GenerateCodesAsync(number, length), Times.Once);
    }

    [Fact]
    public async Task GenerateCodes_ShouldReturnFalse_WhenServiceReturnsFalse()
    {
        // Arrange
        int number = 10;
        byte length = 7;
        _mockService.Setup(s => s.GenerateCodesAsync(number, length)).ReturnsAsync(false);

        // Act
        var result = await _sut.GenerateCodes(number, length);

        // Assert
        Assert.False(result);
        _mockService.Verify(s => s.GenerateCodesAsync(number, length), Times.Once);
    }

    [Fact]
    public async Task ApplyCode_ShouldReturnZero_WhenCodeIsNull()
    {
        // Act
        var result = await _sut.ApplyCode(null);

        // Assert
        Assert.Equal((byte)0, result);
        _mockService.Verify(s => s.ApplyCodeAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ApplyCode_ShouldReturnZero_WhenCodeIsEmpty()
    {
        // Act
        var result = await _sut.ApplyCode("");

        // Assert
        Assert.Equal((byte)0, result);
        _mockService.Verify(s => s.ApplyCodeAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ApplyCode_ShouldReturnZero_WhenCodeIsWhitespace()
    {
        // Act
        var result = await _sut.ApplyCode("   ");

        // Assert
        Assert.Equal((byte)0, result);
        _mockService.Verify(s => s.ApplyCodeAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("123456")]
    [InlineData("123456789")]
    public async Task ApplyCode_ShouldReturnZero_WhenCodeIsInvalidLength(string code)
    {
        // Act
        var result = await _sut.ApplyCode(code);

        // Assert
        Assert.Equal((byte)0, result);
        _mockService.Verify(s => s.ApplyCodeAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("12345678")]
    public async Task ApplyCode_ShouldInvokeService_WhenCodeIsValid(string validCode)
    {
        // Arrange
        _mockService.Setup(s => s.ApplyCodeAsync(validCode)).ReturnsAsync((byte)1);

        // Act
        var result = await _sut.ApplyCode(validCode);

        // Assert
        Assert.Equal((byte)1, result);
        _mockService.Verify(s => s.ApplyCodeAsync(validCode), Times.Once);
    }

    [Fact]
    public async Task ApplyCode_ShouldReturnServiceResult_WhenServiceReturnsOne()
    {
        // Arrange
        string validCode = "1234567";
        _mockService.Setup(s => s.ApplyCodeAsync(validCode)).ReturnsAsync((byte)1);

        // Act
        var result = await _sut.ApplyCode(validCode);

        // Assert
        Assert.Equal((byte)1, result);
        _mockService.Verify(s => s.ApplyCodeAsync(validCode), Times.Once);
    }

    [Fact]
    public async Task ApplyCode_ShouldReturnServiceResult_WhenServiceReturnsZero()
    {
        // Arrange
        string validCode = "1234567";
        _mockService.Setup(s => s.ApplyCodeAsync(validCode)).ReturnsAsync((byte)0);

        // Act
        var result = await _sut.ApplyCode(validCode);

        // Assert
        Assert.Equal((byte)0, result);
        _mockService.Verify(s => s.ApplyCodeAsync(validCode), Times.Once);
    }
}