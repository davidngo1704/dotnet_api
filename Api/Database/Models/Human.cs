namespace Api.Database.Models;

public class Human
{
    public long Id { get; set; }
    public string UserName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Password { get; set; } = "";
    public int Salary { get; set; }
    public int ChiTieu { get; set; }
    public string FacebookLink { get; set; } = "";
    public string WebSiteLink { get; set; } = "";
    public string Info { get; set; } = "";
}
