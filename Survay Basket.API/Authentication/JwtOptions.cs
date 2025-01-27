﻿using System.ComponentModel.DataAnnotations;

namespace Survay_Basket.API.Authentication;

public class JwtOptions
{
    public static string SectionName = "Jwt";
    [Required]
    public string Key { get; set; } = string.Empty;
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string Audience { get; set; } = string.Empty;
    [Required]
    [Range(1, int.MaxValue)]
    public int ExpiryMinutes { get; set; }
}
