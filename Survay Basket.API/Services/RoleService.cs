using Survay_Basket.API.Contracts.Roles;

namespace Survay_Basket.API.Services;

public class RoleService(
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisable = false, CancellationToken cancellationToken = default)
    {
        return await _roleManager.Roles
            .Where(r => !r.IsDefault && (!r.IsDeleted || (includeDisable.HasValue && includeDisable!.Value)))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);
    }
    public async Task<Result<RoleClaimsResponse>> GetAsync(string id) 
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleClaimsResponse>(RoleErrors.RoleNotFound);

        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleClaimsResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(e => e.Value));

        return Result.Success(response);
    }
    public async Task<Result<RoleClaimsResponse>> AddAsync(RoleRequest request)
    {

        if (await _roleManager.Roles.AnyAsync(e => e.Name == request.Name))
            return Result.Failure<RoleClaimsResponse>(RoleErrors.DublicatedRole);

        var allowedPermissions = Permissions.GetAllPermissions;

        if (request.Permission.Except(allowedPermissions).Any())
            return Result.Failure<RoleClaimsResponse>(RoleErrors.InvalidPermission);

        var role = request.Adapt<ApplicationRole>();

        var result = await _roleManager.CreateAsync(role);


        if (!result.Succeeded) 
        {
            var error = result.Errors.First();

            return Result.Failure<RoleClaimsResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        
        var permissions = request.Permission.Select(x => new IdentityRoleClaim<string>
        {
            ClaimType = Permissions.Type,
            ClaimValue = x,
            RoleId = role.Id
        });
        
        await _context.AddRangeAsync(permissions);
        await _context.SaveChangesAsync();

        var response = new RoleClaimsResponse(role.Id, role.Name!, role.IsDeleted, request.Permission);

        return Result.Success(response);
    }
    public async Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure(RoleErrors.RoleNotFound);

        if (await _roleManager.Roles.AnyAsync(e => e.Id != id && e.Name == request.Name, cancellationToken))
            return Result.Failure(RoleErrors.DublicatedRole);

        var allowedPermissions = Permissions.GetAllPermissions;

        if (request.Permission.Except(allowedPermissions).Any())
            return Result.Failure<RoleClaimsResponse>(RoleErrors.InvalidPermission);

        role = request.Adapt(role);

        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded) 
        { 
            var error = result.Errors.First();
            return Result.Success(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest)); 
        }

        
        var curPermissions = await _context.RoleClaims
            .Where(e => e.RoleId == id && e.ClaimType == Permissions.Type)
            .Select(e => e.ClaimValue)
            .ToListAsync(cancellationToken);


        // new permissions 
        var newPermission = request.Permission.Except(curPermissions).Select(x => new IdentityRoleClaim<string>
        {
            ClaimValue = x,
            ClaimType = Permissions.Type,
            RoleId = role.Id
        });

        

        var deletedPermissions = curPermissions.Except(request.Permission);


        await _context.RoleClaims
            .Where(e => deletedPermissions.Contains(e.ClaimValue)!)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.AddRangeAsync(newPermission, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    public async Task<Result> ToggleAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure(RoleErrors.RoleNotFound);

        role.IsDeleted = !role.IsDeleted;

        await _roleManager.UpdateAsync(role);

        return Result.Success();
    }
}
