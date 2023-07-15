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

        /// <summary>
        /// Plays the slot machine game
        /// </summary>
        static void PlaySlotMachine()
        {
            //TODO - Implement main game functions
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

