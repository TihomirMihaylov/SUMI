namespace SUMI.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class PolicyIdInDamage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PolicyId",
                table: "Damages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Damages");
        }
    }
}
