using System.ComponentModel.DataAnnotations;

namespace PipsAndStones.LIB.Models;

public class UserDominoInput
{
    [Range(1, int.MaxValue)]
    public int NumberOfDominoesToCreate { get; set; }

    public List<Tuple<int, int>>? DominoStonesProvided { get; set; }
}