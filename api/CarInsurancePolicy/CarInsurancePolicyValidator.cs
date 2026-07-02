using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;

namespace api.CarInsurancePolicy;

public class CarInsurancePolicyValidator : AbstractValidator<CarInsurancePolicyEntity>
{
    // Placa antiga (ABC-1234 / ABC1234) ou Mercosul (ABC1D23)
    private static readonly Regex PlateRegex =
        new(@"^[A-Z]{3}-?\d{4}$|^[A-Z]{3}\d[A-Z]\d{2}$", RegexOptions.Compiled);

    public CarInsurancePolicyValidator()
    {
        RuleFor(p => p.CpfCnpj)
            .NotEmpty().WithMessage("CPF/CNPJ é obrigatório.")
            .Must(BeAValidCpfOrCnpj).WithMessage("CPF/CNPJ inválido.");

        RuleFor(p => p.VehiclePlate)
            .NotEmpty().WithMessage("Placa do veículo é obrigatória.")
            .Must(BeAValidPlate).WithMessage("Placa do veículo inválida. Use o formato ABC-1234 ou ABC1D23.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");

        RuleFor(p => p.StartDate)
            .NotEmpty().WithMessage("Data de início é obrigatória.");

        RuleFor(p => p.EndDate)
            .NotEmpty().WithMessage("Data de fim é obrigatória.")
            .GreaterThan(p => p.StartDate).WithMessage("Data de fim deve ser posterior à data de início.");

        RuleFor(p => p.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }

    private static bool BeAValidPlate(string? plate)
    {
        if (string.IsNullOrWhiteSpace(plate))
        {
            return false;
        }

        return PlateRegex.IsMatch(plate.Trim().ToUpperInvariant());
    }

    private static bool BeAValidCpfOrCnpj(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var digits = new string(value.Where(char.IsDigit).ToArray());

        return digits.Length switch
        {
            11 => IsValidCpf(digits),
            14 => IsValidCnpj(digits),
            _ => false
        };
    }

    private static bool IsValidCpf(string cpf)
    {
        // Rejeita sequências repetidas (ex.: 111.111.111-11)
        if (cpf.Distinct().Count() == 1)
        {
            return false;
        }

        var numbers = cpf.Select(c => c - '0').ToArray();

        var firstCheck = CalculateCheckDigit(numbers, 9, 10);
        if (firstCheck != numbers[9])
        {
            return false;
        }

        var secondCheck = CalculateCheckDigit(numbers, 10, 11);
        return secondCheck == numbers[10];

        static int CalculateCheckDigit(int[] digits, int length, int startWeight)
        {
            var sum = 0;
            for (var i = 0; i < length; i++)
            {
                sum += digits[i] * (startWeight - i);
            }

            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }
    }

    private static bool IsValidCnpj(string cnpj)
    {
        if (cnpj.Distinct().Count() == 1)
        {
            return false;
        }

        var numbers = cnpj.Select(c => c - '0').ToArray();

        int[] firstWeights = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] secondWeights = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var firstCheck = CalculateCheckDigit(numbers, firstWeights);
        if (firstCheck != numbers[12])
        {
            return false;
        }

        var secondCheck = CalculateCheckDigit(numbers, secondWeights);
        return secondCheck == numbers[13];

        static int CalculateCheckDigit(int[] digits, int[] weights)
        {
            var sum = 0;
            for (var i = 0; i < weights.Length; i++)
            {
                sum += digits[i] * weights[i];
            }

            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }
    }
}
