namespace PipsAndStones.LIB.Models;

public record Stone()
{
    private readonly int _firstSide;
    private readonly int _secondSide;

    public Stone(int firstSide, int secondSide) : this()
    {
        _firstSide = firstSide;
        _secondSide = secondSide;
    }
    
    public int GetFirstSide() => _firstSide;
    public int GetSecondSide() => _secondSide;

    public Stone Flip() => new(_secondSide, _firstSide);

    public override string ToString() => $"({_firstSide}, {_secondSide})";
}