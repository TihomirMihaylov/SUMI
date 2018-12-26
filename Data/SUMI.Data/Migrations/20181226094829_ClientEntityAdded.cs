using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SUMI.Data.Migrations
{
    public partial class ClientEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AspNetUsers_ClientId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Policies_AspNetUsers_ClientId1",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Policies_ClientId1",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "Policies");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Policies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    UniversalCitizenNumber = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IsDeleted",
                table: "Clients",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Clients_ClientId",
                table: "Policies",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Clients_OwnerId",
                table: "Vehicles",
                column: "OwnerId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Clients_ClientId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Clients_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "Policies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId1",
                table: "Policies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policies_ClientId1",
                table: "Policies",
                column: "ClientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_AspNetUsers_ClientId",
                table: "Policies",
                column: "ClientId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_OwnerId",
                table: "Vehicles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
