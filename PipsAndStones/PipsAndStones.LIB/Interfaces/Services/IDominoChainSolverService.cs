using PipsAndStones.LIB.Models;

namespace PipsAndStones.LIB.Interfaces.Services;

public interface IDominoChainSolverService
{
    Result<IEnumerable<Stone>> SolveChain(IEnumerable<Stone> stones);
}