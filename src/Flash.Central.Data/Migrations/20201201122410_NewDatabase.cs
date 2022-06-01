using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Flash.Central.Data.Migrations
{
    public partial class NewDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GasStation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    GasPumpCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasStation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terminal",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Camera",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    NetworkAddress = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ApiKey = table.Column<string>(nullable: true),
                    GasStationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camera", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Camera_GasStation_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Visit",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    EventsCount = table.Column<int>(nullable: false),
                    PlateNumber = table.Column<string>(nullable: true),
                    GasStationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visit_GasStation_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CameraRegion",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    TopLeftX = table.Column<int>(nullable: false),
                    TopLeftY = table.Column<int>(nullable: false),
                    BottomRightX = table.Column<int>(nullable: false),
                    BottomRightY = table.Column<int>(nullable: false),
                    TerminalId = table.Column<long>(nullable: true),
                    CameraId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraRegion_Camera_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Camera",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CameraRegion_Terminal_TerminalId",
                        column: x => x.TerminalId,
                        principalTable: "Terminal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecognitionEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    PlateNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    ImageLink = table.Column<string>(maxLength: 100, nullable: true),
                    CameraRegionId = table.Column<long>(nullable: false),
                    VisitId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecognitionEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecognitionEvent_CameraRegion_CameraRegionId",
                        column: x => x.CameraRegionId,
                        principalTable: "CameraRegion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecognitionEvent_Visit_VisitId",
                        column: x => x.VisitId,
                        principalTable: "Visit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "GasStation",
                columns: new[] { "Id", "CreatedDate", "GasPumpCount", "IsDeleted", "Name", "UpdatedDate" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, "Test", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Terminal",
                columns: new[] { "Id", "Code", "CreatedDate", "Description", "IsDeleted", "Name", "UpdatedDate" },
                values: new object[] { 1L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Test", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Camera",
                columns: new[] { "Id", "ApiKey", "CreatedDate", "GasStationId", "IsDeleted", "Login", "Name", "NetworkAddress", "Notes", "Number", "Password", "Port", "UpdatedDate" },
                values: new object[] { new Guid("132f36ee-4f53-4043-a642-233fba6ee8c4"), "Cam1Test", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1L, false, "admin", "Cam1", "127.0.0.1", "Test", 1, "admin", 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "CameraRegion",
                columns: new[] { "Id", "BottomRightX", "BottomRightY", "CameraId", "CreatedDate", "IsDeleted", "TerminalId", "TopLeftX", "TopLeftY", "UpdatedDate" },
                values: new object[] { 1L, 1920, 1080, new Guid("132f36ee-4f53-4043-a642-233fba6ee8c4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Camera_GasStationId",
                table: "Camera",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Camera_IsDeleted",
                table: "Camera",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CameraRegion_CameraId",
                table: "CameraRegion",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraRegion_IsDeleted",
                table: "CameraRegion",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CameraRegion_TerminalId",
                table: "CameraRegion",
                column: "TerminalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasStation_IsDeleted",
                table: "GasStation",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GasStation_Name",
                table: "GasStation",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RecognitionEvent_CameraRegionId",
                table: "RecognitionEvent",
                column: "CameraRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecognitionEvent_IsDeleted",
                table: "RecognitionEvent",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RecognitionEvent_Timestamp",
                table: "RecognitionEvent",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_RecognitionEvent_VisitId",
                table: "RecognitionEvent",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_GasStationId",
                table: "Visit",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_IsDeleted",
                table: "Visit",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_PlateNumber",
                table: "Visit",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_Start",
                table: "Visit",
                column: "Start");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecognitionEvent");

            migrationBuilder.DropTable(
                name: "CameraRegion");

            migrationBuilder.DropTable(
                name: "Visit");

            migrationBuilder.DropTable(
                name: "Camera");

            migrationBuilder.DropTable(
                name: "Terminal");

            migrationBuilder.DropTable(
                name: "GasStation");
        }
    }
}
