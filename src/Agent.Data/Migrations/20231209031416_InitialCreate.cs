using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agent.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "WorldInstances",
                columns: table => new
                {
                    WorldInstanceId = table.Column<string>(type: "TEXT", nullable: false),
                    WorldId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldInstances", x => x.WorldInstanceId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorldInstances_WorldId",
                table: "WorldInstances",
                column: "WorldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorldInstances");
        }
    }
}
