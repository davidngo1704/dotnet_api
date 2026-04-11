namespace Api.Models;

public class CommonRequest
{
    public string? command { get; set; }
}
public class CommonResponse
{
    public string? data { get; set; }
    public bool ok { get; set; }
}