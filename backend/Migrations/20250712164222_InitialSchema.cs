using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerManufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SerialRegex = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerManufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComputerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocalizedName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Computers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComputerManufacturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Specifications = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PurchaseDt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WarrantyExpirationDt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreateDt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Computers_ComputerManufacturers_ComputerManufacturerId",
                        column: x => x.ComputerManufacturerId,
                        principalTable: "ComputerManufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LnkComputerComputerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComputerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComputerStatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignDt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LnkComputerComputerStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LnkComputerComputerStatuses_ComputerStatuses_ComputerStatusId",
                        column: x => x.ComputerStatusId,
                        principalTable: "ComputerStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LnkComputerComputerStatuses_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LnkComputerUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComputerId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignStartDt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    AssignEndDt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LnkComputerUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LnkComputerUsers_Computers_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LnkComputerUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Computers_ComputerManufacturerId",
                table: "Computers",
                column: "ComputerManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_LnkComputerComputerStatuses_ComputerId",
                table: "LnkComputerComputerStatuses",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_LnkComputerComputerStatuses_ComputerStatusId",
                table: "LnkComputerComputerStatuses",
                column: "ComputerStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LnkComputerUsers_ComputerId",
                table: "LnkComputerUsers",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_LnkComputerUsers_UserId",
                table: "LnkComputerUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LnkComputerComputerStatuses");

            migrationBuilder.DropTable(
                name: "LnkComputerUsers");

            migrationBuilder.DropTable(
                name: "ComputerStatuses");

            migrationBuilder.DropTable(
                name: "Computers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ComputerManufacturers");
        }
    }
}
