using MobilePay.Core.Domain.Commons;
using MobilePay.Core.Domain.TransactionAggregate.Dto;

namespace MobilePay.Core.Domain.TransactionAggregate.ValueObjects;

public class MerchantName : Value<MerchantName>
{
    public string Value { get; private set; }
    public MerchantName(string value)
    {
        Value = value;
    }
    public static MerchantName Create(string name, MerchantList merchantList)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainExceptions.InvalidEntityState($"Merchant Name is required");
        if (!merchantList.MerchantIsAvailable(name))
            throw new DomainExceptions.MerchantNameNotExistException($"{name} not exists in Merchants");
        return new MerchantName(merchantList.GetMerchant(name).Name);
    }

    public static implicit operator string(MerchantName value) => value.Value;
}
