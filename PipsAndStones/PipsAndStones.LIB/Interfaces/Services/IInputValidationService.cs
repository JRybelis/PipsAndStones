namespace PipsAndStones.LIB.Interfaces.Services;

public interface IInputValidationService
{
    bool ValidateProvidedPipValue(int userInputPipValue);
    bool ValidateProvidedAmountOfDominoesToCreate(int providedAmountOfDominoesToCreate);
    (bool isValid, string errorMessage) ValidateDominoPipInput(string input);
    (bool isValid, string errorMessage) ValidateNumberOfDominoesInput(string input);
}