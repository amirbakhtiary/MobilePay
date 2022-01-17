using System.ComponentModel.DataAnnotations;
using static Framework.Tools.Attribute;

namespace MobilePay.Endpoints.WebApi.Models;

public class AddTransactionModel
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2,
    ErrorMessage = "{0} must have min length of {2} and max Length of {1}")]
    public string MerchantName { get; set; }

    [Required]
    [Range(0.1, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public decimal Amount { get; set; }

    [TransactionTimeValidation]
    [Required]
    public DateTime Timestamp { get; set; }

}
