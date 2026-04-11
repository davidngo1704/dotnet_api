using System.Text.Json.Serialization;

namespace Api.Models;

public class MexcSpotModels
{
    [JsonPropertyName("makerCommission")]
    public decimal? MakerCommission { get; set; }

    [JsonPropertyName("takerCommission")]
    public decimal? TakerCommission { get; set; }

    [JsonPropertyName("buyerCommission")]
    public decimal? BuyerCommission { get; set; }

    [JsonPropertyName("sellerCommission")]
    public decimal? SellerCommission { get; set; }

    [JsonPropertyName("canTrade")]
    public bool CanTrade { get; set; }

    [JsonPropertyName("canWithdraw")]
    public bool CanWithdraw { get; set; }

    [JsonPropertyName("canDeposit")]
    public bool CanDeposit { get; set; }

    [JsonPropertyName("updateTime")]
    public long? UpdateTime { get; set; }

    [JsonPropertyName("accountType")]
    public string AccountType { get; set; }

    [JsonPropertyName("balances")]
    public List<Balance> Balances { get; set; }

    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; }
}

public class Balance
{
    public string? asset { get; set; }

    public decimal free { get; set; }

    public decimal locked { get; set; }

    public decimal available { get; set; }
}