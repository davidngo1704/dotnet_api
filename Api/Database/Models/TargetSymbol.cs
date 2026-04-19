namespace Api.Database.Models;

public class TargetSymbol
{
    public long Id { get; set; }
    public string Symbol { get; set; } = "";
    public string Info { get; set; } = "";
    public string Website { get; set; } = "";
    public string Description { get; set; } = "";
}
