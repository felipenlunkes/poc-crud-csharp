using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC_CRUD.Migrations
{
    /// <inheritdoc />
    public partial class HealthCheckTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "code",
                table: "HealthStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "code",
                table: "HealthStatuses");
        }
    }
}
