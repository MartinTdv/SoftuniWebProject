using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectingPeople.Data.Migrations
{
    public partial class DeletableEntityAndAuditInfoAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Ratings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Ratings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Ratings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Ratings",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Items",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Items",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "HelpTasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "HelpTasks",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HelpTasks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "HelpTasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_IsDeleted",
                table: "Ratings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Items_IsDeleted",
                table: "Items",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_HelpTasks_IsDeleted",
                table: "HelpTasks",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_IsDeleted",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Items_IsDeleted",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_HelpTasks_IsDeleted",
                table: "HelpTasks");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "HelpTasks");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "HelpTasks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HelpTasks");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "HelpTasks");
        }
    }
}
