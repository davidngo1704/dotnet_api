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