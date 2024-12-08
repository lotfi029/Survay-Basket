﻿namespace Survay_Basket.API.Contracts.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    IList<string> Roles
);
