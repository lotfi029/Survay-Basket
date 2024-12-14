namespace Survay_Basket.API.Abstractions.Consts;

public static class DefaultRoles
{
    public partial class Admin
    {
        public const string Name = nameof(Admin);
        public const string Id = "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e";
        public const string ConcurrencyStamp = "adcbac50-38d5-4c17-90bd-89a5fe7b8896";
    }
    public partial class User
    {
        public const string Name = nameof(User);
        public const string Id = "5895c06f-d555-406b-8dfe-692716db429d";
        public const string ConcurrencyStamp = "f8d821dc-ced0-4c53-ac50-9d1ec179d62e";
    }
}
