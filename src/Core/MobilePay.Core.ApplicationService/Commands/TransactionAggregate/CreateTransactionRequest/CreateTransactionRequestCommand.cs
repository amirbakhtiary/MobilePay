using MediatR;
using System.ComponentModel.DataAnnotations;
using static Framework.Tools.Attribute;

namespace MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;

public class CreateTransactionRequestCommand : IRequest<CreateTransactionRequestCommand>
{
    [Required]
    [StringLength(50, MinimumLength = 2,
        ErrorMessage = "{0} must have min length of {2} and max Length of {1}")]
    public string MerchantName { get; set; }
    public List<TransactionsDto> Transactions { get; set; }
}

public class TransactionsDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Range(0.1, double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
    public decimal Amount { get; set; }

    [Required]
    [TransactionTimeValidation]
    public DateTime Timestamp { get; set; }
}
