using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class RemovedCameraNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Camera");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Camera",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Camera",
                keyColumn: "Id",
                keyValue: new Guid("132f36ee-4f53-4043-a642-233fba6ee8c4"),
                column: "Number",
                value: 1);
        }
    }
}
