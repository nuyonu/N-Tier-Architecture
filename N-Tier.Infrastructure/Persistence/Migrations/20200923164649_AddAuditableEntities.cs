using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace N_Tier.Infrastructure.Persistence.Migrations
{
    public partial class AddAuditableEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CretatedOn",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "CretatedOn",
                table: "TodoItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "TodoLists",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "TodoItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "TodoItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CretatedOn",
                table: "TodoLists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CretatedOn",
                table: "TodoItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
