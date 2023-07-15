namespace SlotMachineGame
{
    public class SymbolInfo
    {
        public double Coefficient { get; set; }
        public double Probability { get; set; }
        public string Display { get; set; } = string.Empty;
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
    }
}
