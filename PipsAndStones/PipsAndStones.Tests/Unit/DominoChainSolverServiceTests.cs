using PipsAndStones.LIB.Models;
using PipsAndStones.Logic.Services;

namespace PipsAndStones.Tests.Unit;

public class DominoChainSolverServiceTests
{
    [Theory]
    [InlineData(/* empty */)]
    [InlineData(1, 6)] // single stone provided
    public void SolveChain_EmptyCollectionOrSingleStone_ReturnsFailureAndCallsForValidInputs(params int[] pips)
    {
        // Arrange
        var solver = new DominoChainSolverService();
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
        var solver = new DominoChainSolverService();
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
    // No matching sides: (1, 2) (3, 4) (5, 6)
    [InlineData(1, 2, 3, 4, 5, 6, "Unable to form a circular chain with the stones provided.")]
    // Cannot form a circle: (1, 2) (3, 1) (4, 5)
    [InlineData(1, 2, 3, 1, 4, 5, "Unable to form a circular chain with the stones provided.")] 
    // Chain ends don't match up: (0, 3) (3, 3) (3, 6)
    [InlineData(0, 3, 3, 3, 3, 6, "Unable to form a circular chain with the stones provided.")]
    public void SolveChain_UnmatchableStones_ReturnsFailureAndCallsForValidInputs(params object[] parameters)
    {
        // Arrange
        var solver = new DominoChainSolverService();
        var stones = new List<Stone>();
        var pips = parameters.Take(parameters.Length - 1).Cast<int>().ToArray();
        var expectedMessage = (string)parameters[^1];

        for (var i = 0; i < pips.Length; i += 2)
        {
            stones.Add(new Stone(pips[i], pips[i + 1]));
        }
        
        // Act
        var result = solver.SolveChain(stones);
        
        // Assert
        Assert.False(result.IsSuccess() as bool?);
        Assert.Equal(expectedMessage, result.GetErrorMessage());
    }
}