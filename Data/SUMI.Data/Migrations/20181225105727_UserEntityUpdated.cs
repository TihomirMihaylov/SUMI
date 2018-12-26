namespace SUMI.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UserEntityUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Policies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AgentId",
                table: "Policies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgentId1",
                table: "Policies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId1",
                table: "Policies",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversalCitizenNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policies_AgentId1",
                table: "Policies",
                column: "AgentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Policies_ClientId1",
                table: "Policies",
                column: "ClientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_AspNetUsers_AgentId1",
                table: "Policies",
                column: "AgentId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_AspNetUsers_ClientId1",
                table: "Policies",
                column: "ClientId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AspNetUsers_AgentId1",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AspNetUsers_ClientId1",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Policies_AgentId1",
                table: "Policies");

            migrationBuilder.DropIndex(
                name: "IX_Policies_ClientId1",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "AgentId1",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UniversalCitizenNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Policies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AgentId",
                table: "Policies",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
