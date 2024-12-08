using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survay_Basket.API.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Votes_VoteId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_VoteId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "VoteId",
                table: "Answers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<int>(
                name: "VoteId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_VoteId",
                table: "Answers",
                column: "VoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Votes_VoteId",
                table: "Answers",
                column: "VoteId",
                principalTable: "Votes",
                principalColumn: "Id");
        }
    }
}
