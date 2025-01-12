using PipsAndStones.LIB.Models;
using PipsAndStones.Logic.Services;

namespace PipsAndStones.Tests.Unit;

public class DominoChainSolverTests
{
    [Theory]
    [InlineData(/* empty */)]
    [InlineData(1, 6)] // single stone provided
    public void SolveChain_EmptyCollectionOrSingleStone_ReturnsFailureAndCallsForValidInputs(params int[] pips)
    {
        // Arrange
        var solver = new DominoChainSolver();
        IEnumerable<Stone> stones = pips.Length == 0
            ? []
            : [new Stone(pips[0], pips[1])];

        // Act
        var result = solver.SolveChain(stones);
        
        // Assert
        Assert.False(result.IsSuccess() as bool?);
        Assert.Equal("The input provided is invalid. Please provide at least two sets of digits for dominoes.", result.GetErrorMessage());
    }

    [Theory]
    [InlineData(3, 2, 2, 3, 3, 3)] // Three domino stones: (1, 2) (2, 3), (3, 1)
    [InlineData(1, 2, 2, 1)] // Two domino stones: (1, 2) (2, 1)
    [InlineData(2, 3, 3, 4 , 4, 5 , 5, 2)] // Four domino stones: (2, 3) (3, 4) (4, 5) (5, 2)
    public void SolveChain_TwoOrMoreMatchingStones_ReturnsSuccessWithValidChain(params int[] pips)
    {
        // Arrange
        var solver = new DominoChainSolver();
        var stones = new List<Stone>();

        for (var i = 0; i <pips.Length; i+= 2)
        {
            stones.Add(new Stone(pips[i], pips[i + 1]));
        }
        
        // Act
        var result = solver.SolveChain(stones);
        
        // Assert
        Assert.True(result.IsSuccess() as bool?);
        var chain = result.GetValue()!.ToList();
        
        Assert.Equal(stones.Count, chain.Count); // Check chain size
        // Check that each stone connects to the next
        for (var i = 0; i < chain.Count; i++)
        {
            var currentStone = chain[i];
            var nextStone = chain[(i + 1) % chain.Count]; // remainder shows which stone to connect to
            Assert.Equal(currentStone.GetSecondSide(), nextStone.GetFirstSide());
        }
    }

    [Theory]
    [InlineData(1, 2, 3, 4, 5, 6)] // No matching sides: (1, 2) (3, 4) (5, 6)
    [InlineData(1, 2, 3, 1, 4, 5)] // Cannot form a circle: (1, 2) (3, 1) (4, 5)
    [InlineData(0, 3, 3, 3, 3, 6)] // Chain ends don't match up: (0, 3) (3, 3) (3, 6)
    public void SolveChain_UnmatchableStones_ReturnsFailureAndCallsForValidInputs(params int[] pips)
    {
        // Arrange
        var solver = new DominoChainSolver();
        var stones = new List<Stone>();

        for (var i = 0; i < pips.Length; i += 2)
        {
            stones.Add(new Stone(pips[i], pips[i + 1]));
        }
        
        // Act
        var result = solver.SolveChain(stones);
        
        // Assert
        Assert.False(result.IsSuccess() as bool?);
        Assert.Equal("Unable to form a circular chain with the provided domino stones.", result.GetErrorMessage());
    }
}