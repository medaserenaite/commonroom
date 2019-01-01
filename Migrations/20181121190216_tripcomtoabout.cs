using Microsoft.EntityFrameworkCore.Migrations;

namespace commonroom.Migrations
{
    public partial class tripcomtoabout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Trips",
                newName: "About");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "About",
                table: "Trips",
                newName: "Comment");
        }
    }
}
