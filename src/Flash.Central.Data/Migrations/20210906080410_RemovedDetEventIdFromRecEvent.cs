using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flash.Central.Data.Migrations
{
    public partial class RemovedDetEventIdFromRecEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetectionEventId",
                table: "RecognitionEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DetectionEventId",
                table: "RecognitionEvent",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
