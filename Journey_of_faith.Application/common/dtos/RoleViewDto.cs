namespace Journey_of_faith.Application.common.dtos;

public sealed class RoleViewDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public sealed class RolePermissionDto
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? ClaimType { get; set; }
    public string? ClaimValue { get; set; }
}
