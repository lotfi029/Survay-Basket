using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Survay_Basket.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5895c06f-d555-406b-8dfe-692716db429d", "f8d821dc-ced0-4c53-ac50-9d1ec179d62e", true, false, "User", "USER" },
                    { "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e", "adcbac50-38d5-4c17-90bd-89a5fe7b8896", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bf301080-df71-4f1f-a9dc-e3b72c0af129", 0, "78d35802-f198-4baf-92ff-e719824c9977", "admin@survay-basket.com", true, "Survay", "Admin", false, null, "ADMIN@SURVAY-BASKET.COM", "ADMIN@SURVAY-BASKET.COM", "AQAAAAIAAYagAAAAEGGazIzGMOSb0LidgAkPx5j3DXx08kxbPRNElSZk855KI7jGHHVK0y3nvo3UxkzH1Q==", null, false, "AB25ACC0CFEE4807BFE4FBDDBEB46B0C", false, "admin@survay-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "polls:read", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 2, "permissions", "polls:add", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 3, "permissions", "polls:update", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 4, "permissions", "polls:delete", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 5, "permissions", "questions:read", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 6, "permissions", "questions:add", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 7, "permissions", "questions:update", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 8, "permissions", "users:read", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 9, "permissions", "users:add", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 10, "permissions", "users:update", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 11, "permissions", "roles:read", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 12, "permissions", "roles:add", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 13, "permissions", "roles:update", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" },
                    { 14, "permissions", "results:read", "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e", "bf301080-df71-4f1f-a9dc-e3b72c0af129" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5895c06f-d555-406b-8dfe-692716db429d");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e", "bf301080-df71-4f1f-a9dc-e3b72c0af129" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a553ce6-d153-4f9e-b7cf-d25e9b212e6e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bf301080-df71-4f1f-a9dc-e3b72c0af129");
        }
    }
}
