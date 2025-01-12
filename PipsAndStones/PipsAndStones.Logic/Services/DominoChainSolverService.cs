using PipsAndStones.LIB.Interfaces.Services;
using PipsAndStones.LIB.Models;

namespace PipsAndStones.Logic.Services;

public class DominoChainSolverService : IDominoChainSolverService
{
    private class DominoNode(Stone stone)
    {
        public DominoNode? Next { get; set; }

        public Stone GetStone() => stone;
    }
    
    public Result<IEnumerable<Stone>> SolveChain(IEnumerable<Stone> stones)
    {
        var stonesList = stones.ToList();

        if (stonesList.Count < 2)
            return Result<IEnumerable<Stone>>.Failure(
                "The input provided is invalid. Please provide at least two sets of digits for dominoes.");
        
        var head = TryBuildCircularChain(stonesList);

        return head is not null
            ? Result<IEnumerable<Stone>>.Success(BuildChainFromNode(head))
            : Result<IEnumerable<Stone>>.Failure("Unable to form a circular chain with the stones provided.");
    }

    private static DominoNode? TryBuildCircularChain(List<Stone> availableStones, DominoNode? currentChain = null,
        DominoNode? head = null)
    {
        // Base case: all stones used and chain is circular
        if (availableStones.Count == 0)
            return CompleteTheCircle(currentChain, head);
        
        // Try each remaining stone
        for (var i = 0; i < availableStones.Count; i++)
        {
            var stone = availableStones[i];
            availableStones.RemoveAt(i);
            
            var node = TryBuildingWithStoneInOriginalOrientation(availableStones, currentChain, head, stone);
            if (node is not null) 
                return node;

            node = TryBuildingWithStoneOrientationFlipped(availableStones, currentChain, head, stone);
            if (node != null) 
                return node;

            availableStones.Insert(i, stone);
        }
        
        return null;
    }
    
    private static DominoNode? CompleteTheCircle(DominoNode? currentChain, DominoNode? head)
    {
        if (currentChain is null || head is null ||
            currentChain.GetStone().GetSecondSide() != head.GetStone().GetFirstSide()) return null;
            
        currentChain.Next = head; // Complete the circle
        return head;
    }

    private static DominoNode? TryBuildingWithStoneInOriginalOrientation(List<Stone> availableStones, DominoNode? currentChain, DominoNode? head, Stone stone)
    {
        if (currentChain is not null && currentChain.GetStone().GetSecondSide() != stone.GetFirstSide()) 
            return null;
        
        var newNode = new DominoNode(stone);
        if (currentChain is not null)
        {
            currentChain.Next = newNode;
        }
        var result = TryBuildCircularChain(availableStones, newNode, head ?? newNode);
        
        return result;
    }
    
    private static DominoNode? TryBuildingWithStoneOrientationFlipped(List<Stone> availableStones, DominoNode? currentChain,
        DominoNode? head, Stone stone)
    {
        var flippedStone = stone.Flip();
        if (currentChain is not null && currentChain.GetStone().GetSecondSide() != flippedStone.GetFirstSide()) 
            return null;
        
        var newNode = new DominoNode(flippedStone);
        if (currentChain is not null) 
            currentChain.Next = newNode;

        var result = TryBuildCircularChain(availableStones, newNode, head ?? newNode);
        
        return result;
    }

    private static IEnumerable<Stone> BuildChainFromNode(DominoNode head)
    {
        var result = new List<Stone>();
        var current = head;
        
        do
        {
            result.Add(current.GetStone());
            current = current.Next!;
        } while (current != head);

        return result;
    }
}

