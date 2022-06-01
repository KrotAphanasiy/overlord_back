using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class AddingVisitsTrusworthyness : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Trustworthy",
                table: "Visit",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Trustworthy",
                table: "Visit");
        }
    }
}
