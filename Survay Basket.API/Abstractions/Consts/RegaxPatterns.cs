﻿namespace Survay_Basket.API.Abstractions.Consts;

public static class RegaxPatterns
{
    public const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=.*[A-Z])(?=(.*)).{8,}";
}
