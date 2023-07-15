using System.Text;

namespace SlotMachineGame
{
    class Program
    {
        /// <summary>
        /// Declared prefered symbol coefficients and probabilities
        /// </summary>
        static Dictionary<char, SymbolInfo> MachineGameSymbols = new Dictionary<char, SymbolInfo>
        {
            { 'A', new SymbolInfo { Coefficient = 0.4, Probability = 0.45} },
            { 'B', new SymbolInfo { Coefficient = 0.6, Probability = 0.35 } },
            { 'P', new SymbolInfo { Coefficient = 0.8, Probability = 0.15 } },
            { '*', new SymbolInfo { Coefficient = 0, Probability = 0.05 } }
        };

        /// <summary>
        /// Symbol information class
        /// </summary>
        class SymbolInfo
        {
            public double Coefficient { get; set; }
            public double Probability { get; set; }
        }


        //Function to generate a random symbol based on its probability
        static char GenerateSymbol()
        {
            double randNum = new Random().NextDouble();
            double cumulativeProb = 0;
            foreach (var symbol in MachineGameSymbols)
            {
                cumulativeProb += symbol.Value.Probability;
                if (randNum <= cumulativeProb)
                    return symbol.Key;
            }
            return ' ';
        }

        //Function to calculate the win amount based on the winning line(s)
        static double CalculateWin(string line, double stake)
        {
            double winCoefficient = 0;
            foreach (char symbol in line)
            {
                winCoefficient += MachineGameSymbols[symbol].Coefficient;
            }
            return stake * winCoefficient;
        }

        //Function to display the slot machine grid
        static void DisplayGrid(char[,] grid)
        {
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        //Function to check if a line is a winning line
        static bool IsWinningLine(string line)
        {
            HashSet<char> symbols = new HashSet<char>(line);
            if (symbols.Contains('*'))
                symbols.Remove('*');
            return symbols.Count == 1;
        }

        //Function to animate the spinning effect
        static void AnimateSpinning()
        {
            Console.CursorVisible = false;

            char[,] grid = new char[4, 3];
            for (int i = 0; i <= 3; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    grid[i, j] = GenerateSymbol();
                }
            }

            // Perform spinning animation for a few iterations
            for (int iteration = 0; iteration < 10; iteration++)
            {
                // Update grid symbols for spinning effect
                for (int i = 0; i <= 3; i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        grid[i, j] = GenerateSymbol();
                    }
                }

                //Display the grid
                Console.Clear();
                DisplayGrid(grid);

                //Wait for a short interval to create the spinning effect
                Thread.Sleep(200);
            }

            Console.CursorVisible = true;
        }

        /// <summary>
        /// Plays the slot machine game
        /// </summary>
        static void PlaySlotMachine()
        {
            Console.Write("Please deposit money you would like to play with: ");
            double deposit = Convert.ToDouble(Console.ReadLine());
            double balance = deposit;

            while (balance > 0)
            {
                Console.Write("Enter stake amount: ");
                double stake = Convert.ToDouble(Console.ReadLine());

                //Animate spinning effect
                AnimateSpinning();

                //Generate the final slot machine grid
                char[,] grid = new char[4, 3];
                for (int i = 0; i <= 3; i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        grid[i, j] = GenerateSymbol();
                    }
                }

                // Display the final grid
                Console.Clear();
                DisplayGrid(grid);

                // Check for winning lines
                List<string> winnings = new List<string>();
                for (int i = 0; i <= 3; i++)
                {
                    string row = "";
                    for (int j = 0; j <= 2; j++)
                    {
                        row += grid[i, j];
                    }
                    if (IsWinningLine(row))
                    {
                        winnings.Add(row);
                    }
                }

                //Calculate and display the win amount
                double winAmount = 0;
                foreach (string line in winnings)
                {
                    winAmount += CalculateWin(line, stake);
                }
                balance = (balance - stake) + winAmount;
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

