using System.Net;
using FluentAssertions;
using RestSharp;
using Xunit;
using Polly;
namespace ResilientOrders.IntegrationTests;

[Collection("api")]
public class OrdersEndpointTests
{
    private readonly ApiFactoryFixture _fixture;
    private readonly RestClient _client;

    public OrdersEndpointTests(ApiFactoryFixture fixture)
    {
        _fixture = fixture;
        _client = new RestClient(_fixture.HttpClient);
    }

    [Fact(DisplayName = "GET /api/orders retorna 200 OK")]
    public async Task GetOrders_ReturnsOk()
    {
        // Arrange
        var request = new RestRequest("/api/orders", Method.Get);

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNull();
    }

    [Fact(DisplayName = "POST /api/orders com payload válido retorna 201 Created")]
    public async Task PostOrder_WithValidPayload_Returns201()
    {
        // Arrange
        var request = new RestRequest("/api/orders", Method.Post);

        var payload = new
        {
            customerName = "Maria",
            items = new[]
            {
                new
                {
                    productId = 1,
                    productName = "Mouse",
                    unitPrice = 50,
                    quantity = 2
                }
            },
            discountRate = 0.1
        };

        request.AddJsonBody(payload);

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Should().Contain("total");
    }

    [Fact(DisplayName = "POST /api/orders sem itens retorna 400 BadRequest")]
    public async Task PostOrder_WithEmptyItems_Returns400()
    {
        // Arrange
        var request = new RestRequest("/api/orders", Method.Post);

        var payload = new
        {
            customerName = "Maria",
            items = new object[] { }
        };

        request.AddJsonBody(payload);

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

[Fact(DisplayName = "Polly: deve tentar 3 vezes antes de falhar")]
public async Task Polly_DeveTentar3Vezes()
{
    // Arrange
    var attempts = 0;

    var policy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            3,
            attempt => TimeSpan.FromMilliseconds(50 * attempt),
            (ex, time, retry, context) =>
            {
                attempts++;
            });

    Func<Task> act = async () =>
    {
        await policy.ExecuteAsync(() =>
        {
            throw new Exception("Falha simulada");
        });
    };

    // Act + Assert
    await act.Should().ThrowAsync<Exception>();

    attempts.Should().Be(3);
}

}