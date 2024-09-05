using System.Media;

namespace SlotMachineGame
{
    class Program
    {
        static void Main()
        {
            var parentDirectory = string.Empty;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to the Slot Machine Game!\n");
            Console.ResetColor();

            var dir = Directory.GetCurrentDirectory();
            if (dir != null)
            {
                var binIndex = dir.IndexOf("\\src");
                if (binIndex >= 0)
                {
                    parentDirectory = dir[..binIndex];
                    if (OperatingSystem.IsWindows())
                    {
                        var welcomeSound = @$"{parentDirectory}\Media\Windows Unlock.wav";
                        var winSound = new SoundPlayer(welcomeSound);
                        winSound.Play();
                    }
                }
                else
                {
                    Console.WriteLine("Error: Could not find the parent directory.");
                }
            }
            else
            {
                Console.WriteLine("Error: Could not get the current directory.");
            }

            Console.Write("Please enter the amount of money you would like to play with: ");
            // TODO: Create an InputService class in a separate file (e.g., src/InputService.cs)
            InputService inputService = new InputService();
            double depositAmount = inputService.GetValidatedInput(10, 2000000);

            SlotMachine slotMachine = new(parentDirectory);
            slotMachine.PlaySlotMachine(depositAmount);
        }
    }
}

