using System.Text.Json.Serialization;

namespace Api.Models;

public class MexcSpotModels
{

    public List<MexcBalance>? balances { get; set; }
}

public class MexcBalance
{
    public string? asset { get; set; }

    public decimal free { get; set; }

    public decimal locked { get; set; }

    public decimal available { get; set; }
}

public class KucoinBalanceResponse
{
    public List<KucoinBalanceItem>? balances { get; set; }
}

public class KucoinBalanceItem
{
    public string? currency { get; set; }

    public decimal balance { get; set; }

    public decimal available { get; set; }

    public decimal holds { get; set; }
}

public class CryptoModel
{
    public string? asset { get; set; }

    public decimal free { get; set; }

    public decimal total { get; set; }
}
public class BitgetModel
{
    public List<BitgetItem>? data { get; set; }
}
public class BitgetItem 
{
    public string? coin { get; set; }
    public decimal available { get; set; }
}
public class GateSpotModels
{

    public List<MexcBalance>? balances { get; set; }
}

public class GateBalance
{
    public string? asset { get; set; }

    public decimal free { get; set; }

    public decimal locked { get; set; }

    public decimal total { get; set; }
}
public class CoinModel
{
    public string? asset { get; set; }
    public decimal total { get; set; }
    public decimal price { get; set; }
    public decimal value { get; set; }
}
public class CoinPriceModel
{
    public string? symbol { get; set; }
    public decimal price { get; set; }
}
public class CoinListModel
{
    public string? symbol { get; set; }
    public string? price { get; set; }
    public DateTime timeStamp { get; set; }
}
public class CoinPriceResponse
{
    public string? datetime { get; set; }
    public CoinPriceModel? data { get; set; }
}
public class KeyModel
{
    public string? ApiKey { get; set; }
    public string? SecretKey { get; set; }

}
//---------Binance
public class AccountInfo
{
    public int MakerCommission { get; set; }
    public int TakerCommission { get; set; }
    public int BuyerCommission { get; set; }
    public int SellerCommission { get; set; }

    public CommissionRates? CommissionRates { get; set; }

    public bool CanTrade { get; set; }
    public bool CanWithdraw { get; set; }
    public bool CanDeposit { get; set; }
    public bool Brokered { get; set; }
    public bool RequireSelfTradePrevention { get; set; }
    public bool PreventSor { get; set; }

    public long UpdateTime { get; set; }
    public string? AccountType { get; set; }

    public List<Balance>? Balances { get; set; }

    public List<string>? Permissions { get; set; }
    public long Uid { get; set; }
}

public class CommissionRates
{
    public decimal Maker { get; set; }
    public decimal Taker { get; set; }
    public decimal Buyer { get; set; }
    public decimal Seller { get; set; }
}

public class Balance
{
    public string? Asset { get; set; }
    public decimal Free { get; set; }
    public decimal Locked { get; set; }
    public decimal Price { get; set; }
}
//--------------Bingx
public class BingxApiResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("msg")]
    public string Msg { get; set; }

    [JsonPropertyName("data")]
    public List<Position> Data { get; set; }
}

public class Position
{
    [JsonPropertyName("positionId")]
    public string PositionId { get; set; }

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("positionAmt")]
    public string PositionAmt { get; set; }

    [JsonPropertyName("availableAmt")]
    public string AvailableAmt { get; set; }

    [JsonPropertyName("positionSide")]
    public string PositionSide { get; set; }

    [JsonPropertyName("isolated")]
    public bool Isolated { get; set; }

    [JsonPropertyName("avgPrice")]
    public string AvgPrice { get; set; }

    [JsonPropertyName("initialMargin")]
    public string InitialMargin { get; set; }

    [JsonPropertyName("margin")]
    public string Margin { get; set; }

    [JsonPropertyName("leverage")]
    public int Leverage { get; set; }

    [JsonPropertyName("unrealizedProfit")]
    public string UnrealizedProfit { get; set; }

    [JsonPropertyName("realisedProfit")]
    public string RealisedProfit { get; set; }

    [JsonPropertyName("liquidationPrice")]
    public decimal LiquidationPrice { get; set; }

    [JsonPropertyName("pnlRatio")]
    public string PnlRatio { get; set; }

    [JsonPropertyName("maxMarginReduction")]
    public string MaxMarginReduction { get; set; }

    [JsonPropertyName("riskRate")]
    public string RiskRate { get; set; }

    [JsonPropertyName("markPrice")]
    public string MarkPrice { get; set; }

    [JsonPropertyName("positionValue")]
    public string PositionValue { get; set; }

    [JsonPropertyName("onlyOnePosition")]
    public bool OnlyOnePosition { get; set; }

    [JsonPropertyName("createTime")]
    public long CreateTime { get; set; }

    [JsonPropertyName("updateTime")]
    public long UpdateTime { get; set; }

    [JsonPropertyName("minIncreaseMargin")]
    public string MinIncreaseMargin { get; set; }
}