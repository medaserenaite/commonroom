using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace commonroom.Migrations
{
    public partial class manytomanyUPD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedTrips",
                columns: table => new
                {
                    SavedTripID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SavedTripName = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false),
                    TripID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedTrips", x => x.SavedTripID);
                    table.ForeignKey(
                        name: "FK_SavedTrips_Trips_TripID",
                        column: x => x.TripID,
                        principalTable: "Trips",
                        principalColumn: "TripID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedTrips_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedTrips_TripID",
                table: "SavedTrips",
                column: "TripID");

            migrationBuilder.CreateIndex(
                name: "IX_SavedTrips_UserID",
                table: "SavedTrips",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedTrips");
        }
    }
}
