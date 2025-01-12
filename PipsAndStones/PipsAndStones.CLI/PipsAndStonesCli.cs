using PipsAndStones.Interfaces;
using PipsAndStones.LIB.Interfaces.Services;
using PipsAndStones.LIB.Interfaces.Services.IO;
using PipsAndStones.LIB.Models;

namespace PipsAndStones;

public class PipsAndStonesCli(
    IWriter writer,
    IReader reader,
    IInputValidationService inputValidationService,
    IDominoChainSolverService dominoChainSolverService)
    : IPipsAndStonesCli, IPipsAndStonesViewService
{
    public void Run()
    {
        var isRunning = true;

        do
        {
            DisplayMainMenu();

            var userInput = CollectDominoValues();
            
            DisplayTheDominoChain(userInput);

            if (!IsUserContinuing()) 
                isRunning = false;
        } while (isRunning);
    }
    
    public void DisplayMainMenu()
    {
        DisplayConsoleAppHeader();
        writer.Write("    ^ ^                 ");
        writer.Write("   (O,O)                ");
        writer.Write("  (   ) How it works    ");
        writer.Write(@"-"" - ""------------------");
        writer.Write("Enter the number of domino stones you want to provide.");
        writer.Write("Enter the number of pips per domino side for each of your desired domino stones.");
        writer.Write("The app will will review the dominoes provided and let you know if they can be chained back to back into a circle.");
        writer.Write(Environment.NewLine);
        writer.Write(@"█▓▒▒░░░Try it:░░░▒▒▓█");        
    }
    
    private void DisplayConsoleAppHeader()
    {
        writer.Clear();
        writer.Write(@"______ _                             _   _____ _                                      //\                      //\");
        writer.Write(@"| ___ (_)                           | | /  ___| |                                    //  \                    //. \");
        writer.Write(@"| |_/ /_ _ __  ___    __ _ _ __   __| | \ `--.| |_ ___  _ __   ___  ___             // .' \                  //.   \");    
        writer.Write(@"|  __/| | '_ \/ __|  / _` | '_ \ / _` |  `--. \ __/ _ \| '_ \ / _ \/ __|           //\' .'/                 //\   '/");    
        writer.Write(@"| |   | | |_) \__ \ | (_| | | | | (_| | /\__/ / || (_) | | | |  __/\__ \          //  \' /                 //  \ '/");    
        writer.Write(@"\_|   |_| .__/|___/  \__,_|_| |_|\__,_| \____/ \__\___/|_| |_|\___||___/         //    \/                 // '  \/");    
        writer.Write(@"        | |                                                                      \\'   /                  \\ '  /");    
        writer.Write(@"        |_|                                                                       \\ '/                    \\' /");    
        writer.Write(@"                                                                                   \\/                      \\/");    
        writer.Write(Environment.NewLine);    
    }

    private UserDominoInput CollectDominoValues()
    {
        var dominoValues = new UserDominoInput
        {
            DominoStonesProvided = [],
            NumberOfDominoesToCreate = SetAmountOfDominoesToCreate()
        };

        CollectDominoPipValues(dominoValues);

        return dominoValues;
    }
    
    private int SetAmountOfDominoesToCreate()
    {
        while (true) // Loop until valid user input is received
        {
            writer.Write("Please provide an integer value for the amount of dominoes you wish to create:");
            
            if (!int.TryParse(GetUserInput(UserInputType.NumberOfDominoes), out var numberOfDominoesToCreate))
            {
                writer.Write("Invalid input. Please enter a positive integer no lower than 0 and no greater than 28.");
                continue;
            }

            if (!inputValidationService.ValidateProvidedAmountOfDominoesToCreate(numberOfDominoesToCreate))
            {
                writer.Write("Invalid amount. Please enter a number no lower than 0 and no greater than 28.");
                continue;
            }

            return numberOfDominoesToCreate;
        }
    }

    private void CollectDominoPipValues(UserDominoInput dominoValues)
    {
        for (var i = 0; i < dominoValues.NumberOfDominoesToCreate; i++)
        {
            while (true)
            {
                writer.Write($"You have {dominoValues.NumberOfDominoesToCreate - i} of domino stones for creation left to define.");
                writer.Write("Please enter the number of pips for the next domino, separated by a comma:");
                
                var dominoSides = GetUserInput(UserInputType.NumberOfPips).Split(',');
                if (dominoSides.Length != 2)
                {
                    writer.Write("Please provide exactly two integers, separated by a comma.");
                    continue;
                }

                if (!int.TryParse(dominoSides[0], out var firstSide) ||
                    !int.TryParse(dominoSides[1], out var secondSide))
                {
                    writer.Write("Invalid input. Please enter valid integers separated by a comma.");
                    continue;
                }

                if (!inputValidationService.ValidateProvidedPipValue(firstSide)
                    || !inputValidationService.ValidateProvidedPipValue(secondSide))
                {
                    writer.Write("Invalid pip values. Please enter a valid integers separated by a comma.");
                    continue;
                }
                
                dominoValues.DominoStonesProvided!.Add(new Tuple<int, int>(firstSide, secondSide));
                break;
            }
        }
    }

    public string GetUserInput(UserInputType userInputType)
    {
        int maxUserInputLength;
        int minUserInputLength;
        var userInput = string.Empty;
        
        if (userInputType == UserInputType.NumberOfDominoes)
        {
            maxUserInputLength = 2;
            minUserInputLength = 1;
            userInput = reader.Read();

            while (userInput.Length > maxUserInputLength ||
                   userInput.Length < minUserInputLength ||
                   int.TryParse(userInput, out _) == false)
            {
                writer.Write($"Please enter between {minUserInputLength} and {maxUserInputLength} positive digits.");
                userInput = reader.Read();
            }
        }

        if (userInputType == UserInputType.NumberOfPips)
        {
            maxUserInputLength = 3;
            minUserInputLength = 3;
            userInput = reader.Read();

            while (userInput.Length > maxUserInputLength ||
                   userInput.Length < minUserInputLength)
            {
                writer.Write($"Please enter two integers between 0 and 6, separated by a comma.");
                userInput = reader.Read();
            }
        }
        
        return userInput;
    }

    public bool IsUserContinuing()
    {
        string userInput;
        do
        {
            writer.Write($"Would you like to run this again? (Y/N) ");
            userInput = reader.Read().ToUpper();
        } while (!userInput.Equals("Y") && !userInput.Equals("N"));

        if (userInput.Equals("Y"))
            return true;
        
        writer.Write($"Thank you for participating in this domino arrangement effort. Goodbye.");
        
        return false;
    }

    public void DisplayTheDominoChain(UserDominoInput userDominoInput)
    {
        var createdStones = userDominoInput.DominoStonesProvided!.Select(dominoToBeCreated => 
                new Stone(dominoToBeCreated.Item1, dominoToBeCreated.Item2));
        var dominoChain = dominoChainSolverService.SolveChain(createdStones).ToString();
        
        writer.Write(Environment.NewLine);
        writer.Write("Well done. Please see the successful outcome of chaining your dominoes:");
        writer.Write($" {dominoChain}");
        writer.Write(Environment.NewLine);
    }
    
    public enum UserInputType
    {
        NumberOfDominoes,
        NumberOfPips
    }
}