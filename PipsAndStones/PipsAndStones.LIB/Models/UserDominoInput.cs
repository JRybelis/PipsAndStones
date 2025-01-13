using System.ComponentModel.DataAnnotations;

namespace PipsAndStones.LIB.Models;

public class UserDominoInput
{
    private const int MaxDominoes = 28;
    
    [Range(1, MaxDominoes, ErrorMessage = "The number of dominoes must be between 1 and 28.")]
    public int NumberOfDominoesToCreate { get; init; }

    [Required(ErrorMessage = "Domino stones list cannot be null.")]
    public List<Tuple<int, int>>? DominoStonesProvided { get; init; } = [];
}