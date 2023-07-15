using System.Text;

namespace SlotMachineGame
{
    class Program
    {
        /// <summary>
        /// Declared prefered symbol coefficients and probabilities
        /// </summary>
        static Dictionary<char, SymbolInfo> SYMBOLS = new Dictionary<char, SymbolInfo>
        {
            { 'A', new SymbolInfo { Coefficient = 0.4, Probability = 0.45, Display = "🍎", Color = ConsoleColor.Red } },
            { 'B', new SymbolInfo { Coefficient = 0.6, Probability = 0.35, Display = "🍌", Color = ConsoleColor.Yellow } },
            { 'P', new SymbolInfo { Coefficient = 0.8, Probability = 0.15, Display = "🍍", Color = ConsoleColor.Green } },
            { '*', new SymbolInfo { Coefficient = 0, Probability = 0.05, Display = "⭐", Color = ConsoleColor.White } }
        };

        /// <summary>
        /// Symbol information class
        /// </summary>
        class SymbolInfo
        {
            public double Coefficient { get; set; }
            public double Probability { get; set; }
            public string Display { get; set; }
            public ConsoleColor Color { get; set; }
        }

        /// <summary>
        /// Generates a random symbol based on its probability 
        /// </summary>
        /// <returns>char</returns>
        static char GenerateSymbol()
        {
            double randomNum = new Random().NextDouble();
            double cumulativeProbability = 0;
            foreach (var symbol in SYMBOLS)
            {
                cumulativeProbability += symbol.Value.Probability;
                if (randomNum <= cumulativeProbability)
                    return symbol.Key;
            }
            return ' ';
        }

        /// <summary>
        /// Calculates the win amount based on the winning line(s)
        /// </summary>
        /// <param name="line"></param>
        /// <param name="stake"></param>
        /// <returns>Double</returns>
        static double CalculateWin(string line, double stake)
        {
            double winCoefficient = 0;
            foreach (char symbol in line)
            {
                winCoefficient += SYMBOLS[symbol].Coefficient;
            }
            return stake * winCoefficient;
        }

        /// <summary>
        /// Displays our slot machine grid
        /// </summary>
        /// <param name="grid"></param>
        static void DisplayGrid(char[,] grid)
        {
            Console.Clear();
            int numRows = grid.GetLength(0);
            int numColumns = grid.GetLength(1);

            Console.WriteLine("┌─────────────┐");
            for (int row = 0; row <= numRows - 1; row++)
            {
                Console.Write("│");
                for (int col = 0; col <= numColumns - 1; col++)
                {
                    char symbol = grid[row, col];
                    ConsoleColor color = SYMBOLS[symbol].Color;
                    string display = SYMBOLS[symbol].Display;

                    Console.ForegroundColor = color;
                    //TODO - Choose next line based on code editor
                    //Console.WriteLine(symbol); //When using Visual Studio - since Emoji don't work on Console Debugger
                    Console.Write(display); //When using VS Code - Emojis works well
                    Console.ResetColor();
                    Console.Write(" │ ");
                }
                Console.WriteLine();
                if (row < numRows - 1)
                {
                    Console.WriteLine("├─────────────┤");
                }
            }
            Console.WriteLine("└─────────────┘");
        }

        /// <summary>
        /// Checks if a line is a winning line
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Boolean</returns>
        static bool IsWinningLine(string line)
        {
            HashSet<char> uniqueSymbols = new HashSet<char>(line);
            if (uniqueSymbols.Contains('*'))
                uniqueSymbols.Remove('*');
            return uniqueSymbols.Count == 1;
        }

        /// <summary>
        /// Animates the spinning effect
        /// </summary>
        static void AnimateSpinning()
        {
            Console.Clear();
            Console.CursorVisible = false;

            char[,] grid = new char[4, 3];
            for (int row = 0; row <= 3; row++)
            {
                for (int col = 0; col <= 2; col++)
                {
                    grid[row, col] = GenerateSymbol();
                }
            }

            //Do spinning animation for a few iterations
            for (int iteration = 0; iteration < 10; iteration++)
            {
                //Update grid symbols for spinning effect
                for (int row = 0; row <= 3; row++)
                {
                    for (int col = 0; col <= 2; col++)
                    {
                        grid[row, col] = GenerateSymbol();
                    }
                }

                DisplayGrid(grid);

                //Wait for a short interval to create the flashing effect
                Thread.Sleep(200);
            }

            Console.CursorVisible = true;
        }

        /// <summary>
        /// Plays the slot machine game
        /// </summary>
        static void PlaySlotMachine()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Please enter the amount of money you would like to play with: ");
            double depositAmount = Convert.ToDouble(Console.ReadLine());
            double balance = depositAmount;

            while (balance > 0)
            {
                Console.Write("Enter the amount you want to stake: ");
                double stakeAmount = Convert.ToDouble(Console.ReadLine());

                AnimateSpinning();

                //Generate the final slot machine grid
                char[,] grid = new char[4, 3];
                for (int row = 0; row <= 3; row++)
                {
                    for (int col = 0; col <= 2; col++)
                    {
                        grid[row, col] = GenerateSymbol();
                    }
                }

                DisplayGrid(grid);

                //Checks for winning lines
                List<string> winningLines = new List<string>();
                for (int row = 0; row <= 3; row++)
                {
                    string line = "";
                    for (int col = 0; col <= 2; col++)
                    {
                        line += grid[row, col];
                    }
                    if (IsWinningLine(line))
                    {
                        winningLines.Add(line);
                    }
                }

                //Calculates and displays the win amount
                double winAmount = 0;
                foreach (string line in winningLines)
                {
                    winAmount += CalculateWin(line, stakeAmount);
                }
                balance = (balance - stakeAmount) + winAmount;
                Console.WriteLine($"\nYou have won: {winAmount}");
                Console.WriteLine($"Current balance is: {balance}\n");
            }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to the Slot Machine Game!\n");
            Console.ResetColor();
            PlaySlotMachine();
        }
    }
}

