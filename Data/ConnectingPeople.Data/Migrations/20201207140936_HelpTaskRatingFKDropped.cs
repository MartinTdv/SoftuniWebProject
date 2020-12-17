using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectingPeople.Data.Migrations
{
    public partial class HelpTaskRatingFKDropped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HelpTasks_RatingId",
                table: "HelpTasks");

            migrationBuilder.AlterColumn<int>(
                name: "RatingId",
                table: "HelpTasks",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_HelpTasks_RatingId",
                table: "HelpTasks",
                column: "RatingId",
                unique: true,
                filter: "[RatingId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HelpTasks_RatingId",
                table: "HelpTasks");

            migrationBuilder.AlterColumn<int>(
                name: "RatingId",
                table: "HelpTasks",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HelpTasks_RatingId",
                table: "HelpTasks",
                column: "RatingId",
                unique: true);
        }
    }
}
