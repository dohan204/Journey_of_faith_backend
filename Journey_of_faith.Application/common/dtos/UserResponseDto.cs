namespace Journey_of_faith.Application.common.dtos;

public class UserResponseDto
{
    public Guid Id {get; set;}
    public string UserName {get; set;}
    public string Email {get; set;}
    public string Role {get; set;}
    public string Avatar {get; set;}
    public bool? IsDeleted {get; set;}
}
