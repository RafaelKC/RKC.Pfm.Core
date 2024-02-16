using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RKC.Pfm.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Period_State : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "public",
                table: "Periods",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "public",
                table: "Periods");
        }
    }
}
