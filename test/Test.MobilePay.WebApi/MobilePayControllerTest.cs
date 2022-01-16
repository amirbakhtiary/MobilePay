using MobilePay.Core.ApplicationService.Commands.TransactionAggregate.CreateTransactionRequest;
using MobilePay.Endpoints.WebApi.Models;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetMerchantFee;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactionDetail;
using MobilePay.Infrastructures.Data.Queries.TransactionAggregate.GetTransactions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace Test.MobilePay.WebApi;

public class MobilePayControllerTest
{
    private readonly ITestOutputHelper _outputHelper;

    public MobilePayControllerTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public async Task Transaction_Post_List_Responds_OK()
    {
        await using var application = new PlaygroundApplication();
        var jsonString = JsonConvert.SerializeObject(new CreateTransactionRequestCommand
        {
            MerchantName = "Tesla",
            Transactions = new()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 1000,
                    Timestamp = DateTime.Now,
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Amount = 2000,
                    Timestamp = DateTime.Now,
                }
            }
        });

        using var jsonContent = new StringContent(jsonString);
        jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        using var client = application.CreateClient();
        using var response = await client.PostAsync("api/v1/mobilepay/list", jsonContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var transactionListResult = JsonConvert.DeserializeObject<CreateTransactionRequestCommand>(
            await response.Content.ReadAsStringAsync());

        Assert.Equal(2, transactionListResult.Transactions.Count());
    }

    [Theory]
    [InlineData("f4a27289-95aa-4ab4-8e82-d1805c1de5a8")]
    [InlineData("1052c7d6-a041-4c80-86cf-7c3ecbb4a0fc")]
    [InlineData("144e1899-2af8-4b49-847a-a7c207dfd197")]
    public async Task Transaction_Post_Get_Response_OK(string Id)
    {
        await using var application = new PlaygroundApplication();
        var jsonString = JsonConvert.SerializeObject(new AddTransactionModel
        {
            MerchantName = "Tesla",
            Id = Guid.Parse(Id),
            Amount = 1000,
            Timestamp = DateTime.Now,
        });

        using var jsonContent = new StringContent(jsonString);
        jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        using var client = application.CreateClient();
        using var response = await client.PostAsync("api/v1/mobilepay", jsonContent);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var getResponse = await client.GetAsync($"api/v1/mobilepay/{Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var transactionListResult = JsonConvert.DeserializeObject<GetTransactionDetailDto>(
            await getResponse.Content.ReadAsStringAsync());

        Assert.Equal(Guid.Parse(Id), transactionListResult.Id);
    }

    [Fact]
    public async Task Transaction_Post_NotFound()
    {
        await using var application = new PlaygroundApplication();
        using var client = application.CreateClient();
        using var response = await client.GetAsync($"api/v1/mobilepay/442b64df-8554-4f11-b82b-f3f19c1a407a");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("Tesla", 22095)]
    [InlineData("Rema1000", 25041)]
    [InlineData("McDonald", 27987)]
    public async Task Transaction_GetMerchantFee_Result_OK(string merchantName, decimal totalFee)
    {
        await using var application = new PlaygroundApplication();
        var jsonTransactions = JsonConvert.SerializeObject(new CreateTransactionRequestCommand
        {
            MerchantName = merchantName,
            Transactions = GetTransactions()
        });

        using var jsonContentTransactions = new StringContent(jsonTransactions);
        jsonContentTransactions.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        using var client = application.CreateClient();
        using var responseTransactions = await client.PostAsync("api/v1/mobilepay/list", jsonContentTransactions);

        Assert.Equal(HttpStatusCode.OK, responseTransactions.StatusCode);

        using var responseTransactionList = await client.GetAsync($"api/v1/mobilepay?merchantname={merchantName}");

        var transactionListResult = JsonConvert.DeserializeObject<List<GetTransactionsDto>>(
            await responseTransactionList.Content.ReadAsStringAsync());
        Assert.Equal(4, transactionListResult.Count());

        using var responseCalculateFee = await client.GetAsync($"api/v1/mobilepay/calculatefee?merchantname={merchantName}");

        var transactionResult = JsonConvert.DeserializeObject<GetMerchantFeeDto>(
            await responseCalculateFee.Content.ReadAsStringAsync());

        Assert.Equal(totalFee, transactionResult.TotalFee);
    }

    public List<TransactionsDto> GetTransactions() =>
        new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 925000,
                Timestamp = new DateTime(2022, 1, 11),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 756000,
                Timestamp = new DateTime(2022, 1, 11),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 920000,
                Timestamp = new DateTime(2022, 1, 11),
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 345000,
                Timestamp = new DateTime(2022, 1, 11),
            }
        };
}
