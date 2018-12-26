namespace SUMI.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class SpellingMistakeFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FastName",
                table: "AspNetUsers",
                newName: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "FastName");
        }
    }
}
