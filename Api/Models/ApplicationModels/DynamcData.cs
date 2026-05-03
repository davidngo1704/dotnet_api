namespace Api.Models.ApplicationModels;

public class DynamcDataAddModel
{
    public long Id { get; set; }
    public string? TableName { get; set; } = "";
    public string? Code { get; set; } = "";
    public string? Name { get; set; } = "";
    public string? Value { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? JsonValue { get; set; } = "";
    public long ReferenceId { get; set; }

}
public class DynamcDataEditModel
{
    public long Id { get; set; }
    public string? TableName { get; set; } = "";
    public string? Code { get; set; } = "";
    public string? Name { get; set; } = "";
    public string? Value { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? JsonValue { get; set; } = "";
    public long ReferenceId { get; set; }

}
public class DynamcDataViewModel
{
    public long Id { get; set; }
    public string? TableName { get; set; } = "";
    public string? Code { get; set; } = "";
    public string? Name { get; set; } = "";
    public string? Value { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? JsonValue { get; set; } = "";
    public long ReferenceId { get; set; }

}
