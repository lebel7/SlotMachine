using FluentValidation;
using System;
namespace SlotMachineGame;

public class InputService
{
    private readonly IValidator<double> _validator;

    public InputService()
    {
        _validator = new DoubleValidator();
    }

    public double GetValidatedInput(double min, double max)
    {
        while (true)
        {
            Console.Write($"Please enter a number between {min} and {max}: ");
            string userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Error: Input cannot be empty. Please try again.");
                continue;
            }

            var inputArray = userInput.ToCharArray();

            if (!inputArray.All(char.IsDigit))
            {
                Console.WriteLine("Error: Input must contain only digits. Please try again.");
                continue;
            }

            if (!double.TryParse(inputArray, out double input))
            {
                Console.WriteLine("Error: Invalid number format. Please enter a valid number.");
                continue;
            }

            var validationResult = _validator.Validate(input);
            if (!validationResult.IsValid)
            {
                Console.WriteLine($"Error: {validationResult.Errors[0].ErrorMessage}");
                continue;
            }

            if (input < min || input > max)
            {
                Console.WriteLine($"Error: Input must be between {min} and {max}. Please try again.");
                continue;
            }

            return input;
        }
    }
}

public class DoubleValidator : AbstractValidator<double>
{
    public DoubleValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Input cannot be empty.")
            .Must(x => !double.IsNaN(x)).WithMessage("Input must be a valid number.")
            .Must(x => !double.IsInfinity(x)).WithMessage("Input cannot be infinity.")
            .Must(x => x > 0).WithMessage("Input must be a positive number.");
    }
}