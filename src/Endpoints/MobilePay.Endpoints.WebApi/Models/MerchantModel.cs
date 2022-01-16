using System.ComponentModel.DataAnnotations;

namespace MobilePay.Endpoints.WebApi.Models;

public class MerchantModel
{
    [Required]
    public string MerchantName { get; set; }
}
