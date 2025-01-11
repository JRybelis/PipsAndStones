using PipsAndStones.LIB.Models;

namespace PipsAndStones.LIB.Interfaces.Services;

public interface IDominoChainSolver
{
    Result<IEnumerable<Stone>> SolveChain(IEnumerable<Stone> stones);
}