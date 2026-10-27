using System.ComponentModel.DataAnnotations;

namespace TransitDesk.Web.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PastOrPresentDateAttribute : ValidationAttribute
{
    public PastOrPresentDateAttribute()
        : base("Data nie może pochodzić z przyszłości.")
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
            {
                return new ValidationResult(ErrorMessageString);
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Nieprawidłowy format daty.");
    }
}