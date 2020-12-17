using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectingPeople.Data.Migrations
{
    public partial class RatingTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerComment",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "PartnerRating",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "CreatorRatingColorClass",
                table: "Ratings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OthersideComment",
                table: "Ratings",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OthersideRating",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OthersideRatingColorClass",
                table: "Ratings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OthersideUsername",
                table: "Ratings",
                maxLength: 14,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorRatingColorClass",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OthersideComment",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OthersideRating",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OthersideRatingColorClass",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "OthersideUsername",
                table: "Ratings");

            migrationBuilder.AddColumn<string>(
                name: "PartnerComment",
                table: "Ratings",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartnerRating",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
