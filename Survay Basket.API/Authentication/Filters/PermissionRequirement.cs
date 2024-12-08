﻿using Microsoft.AspNetCore.Authorization;

namespace Survay_Basket.API.Authentication.Filters;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;

}
