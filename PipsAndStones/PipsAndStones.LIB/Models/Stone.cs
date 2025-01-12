namespace PipsAndStones.LIB.Models;

public record Stone()
{
    private readonly int _firstSide;
    private readonly int _secondSide;

    private const int MaxPipValue = 6;
    private const int MinPipValue = 0;

    public Stone(int firstSide, int secondSide) : this()
    {
        ValidatePipValue(firstSide, nameof(firstSide));
        ValidatePipValue(secondSide, nameof(secondSide));
        _firstSide = firstSide;
        _secondSide = secondSide;
    }

    private static void ValidatePipValue(int value, string paramName)
    {
        if (value is < MinPipValue or > MaxPipValue)
        {
            throw new ArgumentException($"Pip values must be between {MinPipValue} and {MaxPipValue}", paramName);
        }
    }

    public int GetFirstSide() => _firstSide;
    public int GetSecondSide() => _secondSide;

    public Stone Flip() => new(_secondSide, _firstSide);

    public override string ToString() => $"({_firstSide}, {_secondSide})";
}