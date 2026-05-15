namespace Api.Models.ApplicationModels;

public class ProductAddModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public string? LinkImage { get; set; }
    public string? Value { get; set; }
    public int Gia_Nhap_Vao { get; set; }
    public int Gia_Ban_Ra { get; set; }
    public int Lai_Mong_Muon { get; set; }
}
public class ProductEditModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public string? LinkImage { get; set; }
    public string? Value { get; set; }
    public int Gia_Nhap_Vao { get; set; }
    public int Gia_Ban_Ra { get; set; }
    public int Lai_Mong_Muon { get; set; }
}
public class ProductViewModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public string? LinkImage { get; set; }
    public string? Value { get; set; }
    public int Gia_Nhap_Vao { get; set; }
    public int Gia_Ban_Ra { get; set; }
    public int Lai_Mong_Muon { get; set; }
}
