﻿namespace SUMI.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddingAgentIdToClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgentId",
                table: "Claims",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Claims");
        }
    }
}
