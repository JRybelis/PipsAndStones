using PipsAndStones.LIB.Interfaces.Services;
using PipsAndStones.LIB.Models;

namespace PipsAndStones.Logic.Services;

public class DominoChainSolver : IDominoChainSolver
{
    private class DominoNode
    {
        private readonly Stone _stone;
        public DominoNode? Next { get; set; }

        public DominoNode(Stone stone)
        {
            _stone = stone;
        }
        
        public Stone GetStone() => _stone;
    }
    
    public Result<IEnumerable<Stone>> SolveChain(IEnumerable<Stone> stones)
    {
        throw new NotImplementedException();
    }
}

