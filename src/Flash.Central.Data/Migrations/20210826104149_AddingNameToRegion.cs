using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class AddingNameToRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "CameraRegion",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "CameraRegion");
        }
    }
}
