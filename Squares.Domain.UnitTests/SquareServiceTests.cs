using NSubstitute;
using Squares.Domain.Helpers;
using Squares.Domain.Models;
using Squares.Domain.Repositories;
using Squares.Domain.Services;

namespace Squares.Domain.UnitTests;

public class SquareServiceTests
{
    [Fact]
    public async Task SaveNewSquare_ShouldAddFirstSquareWithPositionZero()
    {
        // Arrange
        var repoSub = Substitute.For<ISquareRepository>();
        var colorGenSub = Substitute.For<IColorGenerator>();

        repoSub.GetLastSquare(Arg.Any<CancellationToken>())
            .Returns((Square)null);

        colorGenSub.GetRandomColorHexString()
            .Returns("#AAAAAA");

        repoSub.SaveNewSquare(Arg.Any<Square>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<Square>());

        var service = new SquareService(repoSub, colorGenSub);

        // Act
        var result = await service.SaveNewSquare(CancellationToken.None);

        // Assert
        Assert.Equal(0, result.Position);
        Assert.False(string.IsNullOrWhiteSpace(result.Color));
    }

    [Fact]
    public async Task SaveNewSquare_ShouldIncrementPosition()
    {
        // Arrange
        var lastSquare = new Square(Color: "#FF0000", Position: 5);
        var repoSub = Substitute.For<ISquareRepository>();
        var colorGenSub = Substitute.For<IColorGenerator>();

        repoSub.GetLastSquare(Arg.Any<CancellationToken>())
            .Returns(lastSquare);

        colorGenSub.GetRandomColorHexString()
            .Returns("#AAAAAA");

        repoSub.SaveNewSquare(Arg.Any<Square>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<Square>());

        var service = new SquareService(repoSub, colorGenSub);

        // Act
        var result = await service.SaveNewSquare(CancellationToken.None);

        // Assert
        Assert.Equal(6, result.Position);
        Assert.False(string.IsNullOrWhiteSpace(result.Color));
    }

    [Fact]
    public async Task SaveNewSquare_ShouldNotRepeatColorOfLastSquare()
    {
        // Arrange
        var lastSquare = new Square(Color: "#AAAAAA", Position: 1);
        var repoSub = Substitute.For<ISquareRepository>();
        var colorGenSub = Substitute.For<IColorGenerator>();

        // Simulate first color is same as last, second is different
        colorGenSub.GetRandomColorHexString()
            .Returns("#AAAAAA", "#BBBBBB");

        repoSub.GetLastSquare(Arg.Any<CancellationToken>())
            .Returns(lastSquare);

        repoSub.SaveNewSquare(Arg.Any<Square>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<Square>());

        var service = new SquareService(repoSub, colorGenSub);

        // Act
        var result = await service.SaveNewSquare(CancellationToken.None);

        // Assert
        Assert.Equal("#BBBBBB", result.Color);
        Assert.NotEqual(lastSquare.Color, result.Color);
    }
}