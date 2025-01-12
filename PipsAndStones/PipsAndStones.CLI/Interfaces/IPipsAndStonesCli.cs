namespace PipsAndStones.Interfaces;

public interface IPipsAndStonesCli
{
    void Run();
    void DisplayMainMenu();
    string GetUserInput(PipsAndStonesCli.UserInputType userInputType);
    bool IsUserContinuing();
}