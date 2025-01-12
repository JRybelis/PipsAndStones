using PipsAndStones.LIB.Models;

namespace PipsAndStones.Tests.Unit;

public class StoneTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 0)]
    [InlineData(2, 6)]
    [InlineData(3, 0)]
    [InlineData(6, 6)]
    [InlineData(6, 4)]
    [InlineData(5, 1)]
    [InlineData(0, 3)]
    public void Constructor_ValidPips_ShouldCreateStone(int firstSide, int secondSide)
    {
        // Act
        var stone = new Stone(firstSide, secondSide);
        
        // Assert
        Assert.Equal(firstSide, stone.GetFirstSide());
        Assert.Equal(secondSide, stone.GetSecondSide());
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    [InlineData(-1, -1)]
    [InlineData(0, 8)]
    [InlineData(8, 0)]
    [InlineData(8, 8)]
    public void Constructor_InvalidPips_ShouldThrowException(int firstSide, int secondSide)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Stone(firstSide, secondSide));
        Assert.Contains("Pip values must be between 0 and 6", exception.Message);
    }

    [Fact]
    public void Flip_WhenCalled_ReturnsNewStoneWithReversedSides()
    {
        // Arrange
        var stone = new Stone(3, 4);
        
        // Act
        var flipped = stone.Flip();
        
        // Assert
        Assert.Equal(4, flipped.GetFirstSide());
        Assert.Equal(3, flipped.GetSecondSide());
        Assert.NotEqual(stone, flipped);
    }

    [Fact]
    public void ToString_WhenCalled_ReturnsCorrectFormat()
    {
        // Arrange
        var stone = new Stone(3, 4);
        
        // Act
        var result = stone.ToString();
        
        // Assert
        Assert.Equal("(3, 4)", result);
    }

    [Fact]
    public void Equals_TwoStonesWithSameValues_ReturnsTrue()
    {
        // Arrange
        var stone1 = new Stone(3, 4);
        var stone2 = new Stone(3, 4);
        
        // Act & Assert
        Assert.Equal(stone1, stone2); // Stone is a record
    }

    [Fact]
    public void Equals_TwoStonesWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        var stone1 = new Stone(3, 4);
        var stone2 = new Stone(3, 1);
        
        // Act & Assert
        Assert.NotEqual(stone1, stone2);
    }
}