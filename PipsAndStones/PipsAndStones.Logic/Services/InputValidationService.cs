using PipsAndStones.LIB.Interfaces.Services;

namespace PipsAndStones.Logic.Services;

public class InputValidationService : IInputValidationService
{
    private const int MinPips = 0;
    private const int MaxPips = 6;
    private const int MaxDominoes = 28; // Standard domino set size
    public bool ValidateProvidedPipValue(int userInputPipValue)
    {
        return userInputPipValue is >= MinPips and <= MaxPips; 
    }

    public bool ValidateProvidedAmountOfDominoesToCreate(int providedAmountOfDominoesToCreate)
    {
        return providedAmountOfDominoesToCreate is > 0 and <= MaxDominoes;
    }

    public (bool isValid, string errorMessage) ValidateDominoPipInput(string input)
    {
        var sides = input.Split(",");
        if (sides.Length != 2)
            return (false, "Please provide exactly two integers, separated by a comma.");

        if (!int.TryParse(sides[0], out var firstSide) ||
            !int.TryParse(sides[1], out var secondSide))
            return (false, "Invalid input. Please enter valid integers separated by a comma.");

        if (!ValidateProvidedPipValue(firstSide) || !ValidateProvidedPipValue(secondSide))
            return (false, $"Invalid pip values. Please enter integers between {MinPips} and {MaxPips}.");
        
        return (true, string.Empty);
    }

    public (bool isValid, string errorMessage) ValidateNumberOfDominoesInput(string input)
    {
        if (!int.TryParse(input, out var number))
            return (false, $"Invalid input. Please enter a number between 1 and {MaxDominoes}.");

        if (!ValidateProvidedAmountOfDominoesToCreate(number))
            return (false, $"Invalid amount. Please enter a number between 1 and {MaxDominoes}.");
        
        return (true, string.Empty);
    }
}