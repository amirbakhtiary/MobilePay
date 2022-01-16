using System.Net;
using Xunit;

namespace Test.MobilePay.WebApi;

public partial class Swagger
{
    [Fact]
    public async Task SwaggerUI_Responds_OK_In_Development()
    {
        await using var application = new PlaygroundApplication("Development");

        var client = application.CreateClient();
        var response = await client.GetAsync("/swagger");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SwaggerUI_Redirects_To_Canonical_Path_In_Development()
    {
        await using var application = new PlaygroundApplication("Development");

        var client = application.CreateClient(new() { AllowAutoRedirect = false });
        var response = await client.GetAsync("/swagger/");

        Assert.Equal(HttpStatusCode.Moved, response.StatusCode);
        Assert.Equal("index.html", response.Headers.Location?.ToString());
    }
}
