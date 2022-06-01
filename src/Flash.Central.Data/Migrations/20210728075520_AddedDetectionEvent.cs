using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class AddedDetectionEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetectionEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Probability = table.Column<double>(type: "double precision", nullable: false),
                    OriginalImageLink = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CroppedImageLink = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CameraRegionId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetectionEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetectionEvent_CameraRegion_CameraRegionId",
                        column: x => x.CameraRegionId,
                        principalTable: "CameraRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetectionEvent_CameraRegionId",
                table: "DetectionEvent",
                column: "CameraRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_DetectionEvent_IsDeleted",
                table: "DetectionEvent",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DetectionEvent_Timestamp",
                table: "DetectionEvent",
                column: "Timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetectionEvent");
        }
    }
}
