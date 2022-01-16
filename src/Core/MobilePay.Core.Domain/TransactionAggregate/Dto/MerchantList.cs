namespace MobilePay.Core.Domain.TransactionAggregate.Dto;

public class MerchantList
{
    public List<Merchant> Merchants { get; }
    public MerchantList(List<Merchant> merchants)
    {
        Merchants = merchants;
    }

    public bool MerchantIsAvailable(string merchant) =>
        Merchants.Any(o => o.Name.Equals(merchant, StringComparison.OrdinalIgnoreCase));

    public Merchant GetMerchant(string merchant) =>
        Merchants.FirstOrDefault(o => o.Name.Equals(merchant, StringComparison.OrdinalIgnoreCase));
}
