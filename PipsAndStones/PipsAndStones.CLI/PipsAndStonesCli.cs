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
            writer.Write($"You have {dominoValues.NumberOfDominoesToCreate - i} of domino stones for creation left to define.");
            writer.Write("Please enter the number of pips for the next domino, separated by a comma:");

            var input = GetUserInput(UserInputType.NumberOfPips);
            var sides = input.Split(',').Select(int.Parse).ToArray();
            dominoValues.DominoStonesProvided!.Add(new Tuple<int, int>(sides[0], sides[1]));
        }
    }

    public string GetUserInput(UserInputType userInputType)
    {
        while (true)
        {
            var input = reader.Read();
            var validationResult = userInputType switch
            {
                UserInputType.NumberOfDominoes => inputValidationService.ValidateNumberOfDominoesInput(input),
                UserInputType.NumberOfPips => inputValidationService.ValidateDominoPipInput(input),
                _ => throw new ArgumentOutOfRangeException(nameof(userInputType), userInputType, null)
            };

            if (validationResult.isValid)
                return input;
            
            writer.Write(validationResult.errorMessage);
        }
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
        var result = dominoChainSolverService.SolveChain(createdStones); 
        
        writer.Write(Environment.NewLine);

        if (result.IsSuccess())
        {
            var chain = result.GetValue()!;
            var chainDisplay = string.Join(" -> ", chain);
            
            writer.Write("Well done. Please see the successful outcome of chaining your dominoes:");
            writer.Write($" {chainDisplay}");
        }
        else
        {
            writer.Write("Unable to form a chain with the provided dominoes.");
            writer.Write($"{result.GetErrorMessage()}");
        }

        writer.Write(Environment.NewLine);
    }
    
    public enum UserInputType
    {
        NumberOfDominoes,
        NumberOfPips
    }
}