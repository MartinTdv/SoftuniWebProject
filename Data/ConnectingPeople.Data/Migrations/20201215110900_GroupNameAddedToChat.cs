using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectingPeople.Data.Migrations
{
    public partial class GroupNameAddedToChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatGroupName",
                table: "Chat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatGroupName",
                table: "Chat");
        }
    }
}
