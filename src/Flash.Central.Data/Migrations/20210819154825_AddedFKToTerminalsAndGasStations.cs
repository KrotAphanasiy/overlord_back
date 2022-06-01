using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class AddedFKToTerminalsAndGasStations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Terminal_GasStationId",
                table: "Terminal",
                column: "GasStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terminal_GasStation_GasStationId",
                table: "Terminal",
                column: "GasStationId",
                principalTable: "GasStation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Terminal_GasStation_GasStationId",
                table: "Terminal");

            migrationBuilder.DropIndex(
                name: "IX_Terminal_GasStationId",
                table: "Terminal");
        }
    }
}
