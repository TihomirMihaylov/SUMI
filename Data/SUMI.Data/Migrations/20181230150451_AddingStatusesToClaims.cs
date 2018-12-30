using Microsoft.EntityFrameworkCore.Migrations;

namespace SUMI.Data.Migrations
{
    public partial class AddingStatusesToClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Claims",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Claims");
        }
    }
}
