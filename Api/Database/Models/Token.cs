namespace Api.Database.Models;

public class Token
{
    public long Id { get; set; }
    public string? TokenValue { get; set; }
    public long? Value { get; set; }
}
