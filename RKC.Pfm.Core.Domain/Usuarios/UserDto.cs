using FirebaseAdmin.Auth;
using RKC.Pfm.Core.Domain.Communs;

namespace RKC.Pfm.Core.Domain.Usuarios;

public class UserDto: EntityDto
{
    public string Name { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public string PhotoUrl { get; }
    public bool EmailVerified { get; }
    public bool Disabled { get; }
    public string TenantId { get; }

    public UserDto(UserRecord userRecord)
    { 
        Id = Guid.Parse(userRecord.Uid);
        Name = userRecord.DisplayName;
        Email = userRecord.Email;
        PhotoUrl = userRecord.PhotoUrl;
        PhoneNumber = userRecord.PhoneNumber;
        EmailVerified = userRecord.EmailVerified;
        Disabled = userRecord.Disabled;
        TenantId = userRecord.TenantId;
    } 
}