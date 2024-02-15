using System;
using Microsoft.EntityFrameworkCore.Migrations;
using RKC.Pfm.Core.Infrastructure.Database.Services;

#nullable disable

namespace RKC.Pfm.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Period : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Periods",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSchema = table.Column<bool>(type: "boolean", nullable: false),
                    IdSchemaPeriod = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periods_Periods_IdSchemaPeriod",
                        column: x => x.IdSchemaPeriod,
                        principalSchema: "public",
                        principalTable: "Periods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Periods_End",
                schema: "public",
                table: "Periods",
                column: "End",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_IdSchemaPeriod",
                schema: "public",
                table: "Periods",
                column: "IdSchemaPeriod");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_Start",
                schema: "public",
                table: "Periods",
                column: "Start",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Periods",
                schema: "public");
        }
    }
}
