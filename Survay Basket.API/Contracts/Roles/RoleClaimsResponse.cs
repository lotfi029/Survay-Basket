using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace Survay_Basket.API.Contracts.Roles;

public record RoleClaimsResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions
);
