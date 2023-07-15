using System.Text;

namespace SlotMachineGame
{
    public class SlotMachine
    {
        private Dictionary<char, SymbolInfo>? MachineGameSymbols;
        private const int NumRows = 4;
        private const int NumColumns = 3;

        public SlotMachine()
        {
            MachineGameSymbols = new Dictionary<char, SymbolInfo>();
            InitializeSymbols();
        }

        private void InitializeSymbols()
        {
            MachineGameSymbols = new Dictionary<char, SymbolInfo>
            {
                { 'A', new SymbolInfo { Coefficient = 0.4, Probability = 0.45, Display = "🍎", Color = ConsoleColor.Red } },
                { 'B', new SymbolInfo { Coefficient = 0.6, Probability = 0.35, Display = "🍌", Color = ConsoleColor.Yellow } },
                { 'P', new SymbolInfo { Coefficient = 0.8, Probability = 0.15, Display = "🍍", Color = ConsoleColor.Green } },
                { '*', new SymbolInfo { Coefficient = 0, Probability = 0.05, Display = "⭐", Color = ConsoleColor.White } }
            };
        }

        public char GenerateSymbol()
        {
            double randomNum = new Random().NextDouble();
            double cumulativeProbability = 0;
            if (MachineGameSymbols == null) return ' ';
            foreach (var symbol in MachineGameSymbols)
            {
                cumulativeProbability += symbol.Value.Probability;
                if (randomNum <= cumulativeProbability)
                    return symbol.Key;
            }
            return ' ';
        }

        public double CalculateWin(string line, double stake)
        {
            double winCoefficient = 0;
            if(MachineGameSymbols == null) return winCoefficient;
            foreach (char symbol in line)
            {
                winCoefficient += MachineGameSymbols[symbol].Coefficient;
            }
            return stake * winCoefficient;
        }

        public static bool IsWinningLine(string line)
        {
            HashSet<char> uniqueSymbols = new(line);
            if (uniqueSymbols.Contains('*'))
                uniqueSymbols.Remove('*');
            return uniqueSymbols.Count == 1;
        }

        public void AnimateSpinning()
        {
            Console.Clear();
            Console.CursorVisible = false;

            char[,] grid = new char[NumRows, NumColumns];
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    grid[row, col] = GenerateSymbol();
                }
            }

            for (int iteration = 0; iteration < 10; iteration++)
            {
                for (int row = 0; row < NumRows; row++)
                {
                    for (int col = 0; col < NumColumns; col++)
                    {
                        grid[row, col] = GenerateSymbol();
                    }
                }

                DisplayGrid(grid);

                Thread.Sleep(200);
            }

            Console.CursorVisible = true;
        }

        public void DisplayGrid(char[,] grid)
        {
            Console.Clear();
            int numRows = grid.GetLength(0);
            int numColumns = grid.GetLength(1);

            Console.WriteLine("┌─────────────┐");
            for (int row = 0; row < numRows; row++)
            {
                Console.Write("│");
                for (int col = 0; col < numColumns; col++)
                {
                    char symbol = grid[row, col];
                    if(MachineGameSymbols != null)
                    {
                        ConsoleColor color = MachineGameSymbols[symbol].Color;
                        string display = MachineGameSymbols[symbol].Display;

                        //TODO - Choose next line based on code editor
                        //Console.WriteLine(symbol); //When using Visual Studio - since Emoji don't work on Console Debugger
                        Console.Write(display); //When using VS Code - Emojis works well**
                        Console.ForegroundColor = color;
                    }
                    
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

        public void PlaySlotMachine(double depositAmount)
        {
            Console.OutputEncoding = Encoding.UTF8;
            double balance = depositAmount;

            while (balance > 0)
            {
                Console.Write("Enter the amount you want to stake: ");
                double stakeAmount = Convert.ToDouble(Console.ReadLine());

                AnimateSpinning();

                char[,] grid = new char[NumRows, NumColumns];
                for (int row = 0; row < NumRows; row++)
                {
                    for (int col = 0; col < NumColumns; col++)
                    {
                        grid[row, col] = GenerateSymbol();
                    }
                }

                DisplayGrid(grid);

                List<string> winningLines = new();
                for (int row = 0; row < NumRows - 1; row++)
                {
                    string line = "";
                    for (int col = 0; col < NumColumns - 1; col++)
                    {
                        line += grid[row, col];
                    }
                    if (IsWinningLine(line))
                    {
                        winningLines.Add(line);
                    }
                }

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
    }
}

