using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectingPeople.Data.Migrations
{
    public partial class AddedLocationForHelpTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "HelpTasks",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "HelpTasks");
        }
    }
}
