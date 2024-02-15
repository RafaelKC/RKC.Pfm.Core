using System.ComponentModel.DataAnnotations;

namespace RKC.Pfm.Core.Infrastructure.Authentication.Dots;

public class LoginInput
{
    [Required(AllowEmptyStrings = false)]
    public string email { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    public string password { get; set; }
}