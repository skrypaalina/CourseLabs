using System.ComponentModel.DataAnnotations;

namespace Lab04ValidationBinding.Models;

public class Product : IValidatableObject
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100_000)]
    public double Price { get; set; }

    [Required, StringLength(60)]
    public string Category { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [EnumDataType(typeof(ProductType))]
    public ProductType Type { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Description)) yield break;

        if (Description.Length <= 2)
            yield return new ValidationResult("Description must be longer than 2 characters.", [nameof(Description)]);

        if (!string.IsNullOrEmpty(Name))
        {
            if (string.Equals(Description, Name, StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("Description must not be the same as the product name.", [nameof(Description)]);

            if (!Description.StartsWith(Name, StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("Description must start with the product name.", [nameof(Description)]);
        }
    }
}
