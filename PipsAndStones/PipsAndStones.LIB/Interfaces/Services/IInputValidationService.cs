namespace PipsAndStones.LIB.Interfaces.Services;

public interface IInputValidationService
{
    bool ValidateProvidedPipValue(int userInputPipValue);
    bool ValidateProvidedAmountOfDominoesToCreate(int providedAmountOfDominoesToCreate);
}