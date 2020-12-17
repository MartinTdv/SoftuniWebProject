using Microsoft.EntityFrameworkCore.Migrations;

namespace ConnectingPeople.Data.Migrations
{
    public partial class HelpTasksAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameInCyrillic = table.Column<string>(nullable: true),
                    FAIconClass = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorRating = table.Column<int>(nullable: false),
                    CreatorComment = table.Column<string>(maxLength: 160, nullable: true),
                    PartnerRating = table.Column<int>(nullable: false),
                    PartnerComment = table.Column<string>(maxLength: 160, nullable: true),
                    TaskId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 950, nullable: false),
                    ImageName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    CreatorId = table.Column<string>(nullable: false),
                    RatingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpTasks_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HelpTasks_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HelpTaskItems",
                columns: table => new
                {
                    HelpTaskId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ItemUseType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpTaskItems", x => new { x.HelpTaskId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_HelpTaskItems_HelpTasks_HelpTaskId",
                        column: x => x.HelpTaskId,
                        principalTable: "HelpTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HelpTaskItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelpTaskItems_ItemId",
                table: "HelpTaskItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpTasks_CreatorId",
                table: "HelpTasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpTasks_RatingId",
                table: "HelpTasks",
                column: "RatingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpTaskItems");

            migrationBuilder.DropTable(
                name: "HelpTasks");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Ratings");
        }
    }
}
