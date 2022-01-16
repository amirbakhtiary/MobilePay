using System.ComponentModel.DataAnnotations;

namespace MobilePay.Endpoints.WebApi.Models;

public class GetTransactionsModel
{
    [Required]
    public string MerchantName { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 10;
}
