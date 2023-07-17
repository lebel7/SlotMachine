using System.Media;

namespace SlotMachineGame
{
    class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to the Slot Machine Game!\n");
            Console.ResetColor();
            if (OperatingSystem.IsWindows())
            {
                var winSound = new SoundPlayer(@"C:\Windows\Media\Windows Unlock.wav");
                winSound.Play();
            }
            
            Console.Write("Please enter the amount of money you would like to play with: ");
            double depositAmount = Convert.ToDouble(Console.ReadLine());

            SlotMachine slotMachine = new();
            slotMachine.PlaySlotMachine(depositAmount);
        }
    }
}

