using NAudio.Wave;
using System.Text;

namespace SlotMachineGame
{
    public class SlotMachine
    {
        private Dictionary<char, SymbolInfo>? MachineGameSymbols;
        private const int NumRows = 4;
        private const int NumColumns = 3;
        private WaveOutEvent? waveOutEvent = null;
        private AudioFileReader? audioFileReader = null;
        private LoopStream? loopStream = null;

        public SlotMachine()
        {
            MachineGameSymbols = new Dictionary<char, SymbolInfo>();
            InitializeSymbols();
            LoadSoundEffects();
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

        private void LoadSoundEffects()
        {
            waveOutEvent = new WaveOutEvent();
            audioFileReader = new AudioFileReader(@"C:\Windows\Media\chimes.wav");
        }

        private void PlaySoundEffect(string filePath)
        {
            audioFileReader?.Dispose();
            audioFileReader = new AudioFileReader(filePath);
            waveOutEvent?.Stop();
            waveOutEvent?.Init(audioFileReader);
            waveOutEvent?.Play();
        }

        public void PlayWinSound()
        {
            PlaySoundEffect(@"C:\Windows\Media\Windows Restore.wav");
        }

        public void PlayLoseSound()
        {
            PlaySoundEffect(@"C:\Windows\Media\recycle.wav");
        }

        private void PlaySpinningSound()
        {
            string filePath = @"C:\Windows\Media\Windows Ringin.wav";

            if (File.Exists(filePath))
            {
                audioFileReader?.Dispose();
                audioFileReader = new AudioFileReader(filePath);

                loopStream?.Dispose();
                loopStream = new LoopStream(audioFileReader, 10);

                waveOutEvent?.Stop();
                waveOutEvent?.Init(loopStream);
                waveOutEvent?.Play();
            }
        }

        public void PlayGameOverSound()
        {
            PlaySoundEffect(@"C:\Windows\Media\Windows Notify Email.wav");
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
            if (MachineGameSymbols == null) return 0;
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
            Task.Run(() => PlaySpinningSound());
            //PlaySpinningSound();
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

            waveOutEvent?.Stop();
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

            while (balance > 0 && balance >= 10)
            {
                Console.Write("Enter the amount you want to stake: ");
                double stakeAmount = Convert.ToDouble(Console.ReadLine());

                // Validate the stake amount
                if (stakeAmount < 10.00 || stakeAmount > balance)
                {
                    Console.WriteLine("Invalid stake amount. Please try again.");
                    continue;
                }

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
                for (int row = 0; row < NumRows; row++)
                {
                    string line = "";
                    for (int col = 0; col < NumColumns; col++)
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

                if (winAmount > 0)
                {
                    PlayWinSound();
                }
                else
                {
                    PlayLoseSound();
                }
            }

            PlayGameOverSound();
            Console.WriteLine("Game Over !!");
            waveOutEvent?.Stop();
        }
    }
}

