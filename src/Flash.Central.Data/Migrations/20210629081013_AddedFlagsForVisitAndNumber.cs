using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class AddedFlagsForVisitAndNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncorrectVisit",
                table: "Visit",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PercentAssurance",
                table: "Visit",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncorrectNumber",
                table: "RecognitionEvent",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncorrectVisit",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "PercentAssurance",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "IsIncorrectNumber",
                table: "RecognitionEvent");
        }
    }
}
