namespace MobilePay.Core.Domain.TransactionAggregate.Dto;

public class FeeSetting
{
    public bool CalculationMethod { get; set; } = true;
    public decimal FixedFee { get; set; } = 1;
    public int VolumeCount { get; set; } = 10;
    public decimal VolumeDiscount { get; set; } = 20;
}

public class FeeSettingDto
{
    public FeeSetting FeeSetting { get; set; }
    public Merchant Merchant { get; set; }
}
