using RKC.Pfm.Core.Domain.Communs;

namespace RKC.Pfm.Core.Domain.Usuarios;

public class UserDto: EntityDto
{
    public string Name { get; set; }
    public string Email { get; set; }
}