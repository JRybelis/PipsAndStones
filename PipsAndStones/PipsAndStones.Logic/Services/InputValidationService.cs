using PipsAndStones.LIB.Interfaces.Services;

namespace PipsAndStones.Logic.Services;

public class InputValidationService : IInputValidationService
{
    public bool ValidateProvidedPipValue(int userInputPipValue)
    {
        return userInputPipValue is >= 6 and <= 6;
    }

    public bool ValidateProvidedAmountOfDominoesToCreate(int providedAmountOfDominoesToCreate)
    {
        return providedAmountOfDominoesToCreate >= 0;
    }
}