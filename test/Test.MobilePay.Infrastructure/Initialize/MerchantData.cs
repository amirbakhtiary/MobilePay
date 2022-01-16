﻿using MobilePay.Core.Domain.TransactionAggregate;
using MobilePay.Core.Domain.TransactionAggregate.Dto;
using System.Collections.Generic;

namespace Test.MobilePay.Infrastructure.Initialize;

public class MerchantData
{
    public static MerchantList GetMerchantList() =>
        new MerchantList(new List<Merchant>
        {
            new Merchant
            {
                Name="Tesla",
                Discount = 25
            },
            new Merchant
            {
                Name="Rema1000",
                Discount = 10
            },
            new Merchant
            {
                Name="McDonald",
                Discount = 5
            }
        });
}
