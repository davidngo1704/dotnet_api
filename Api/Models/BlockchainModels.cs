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