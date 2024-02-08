using System.ComponentModel.DataAnnotations;

namespace RKC.Pfm.Core.Application.Users.Dtos;

public class UserCreateInput
{
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
}