using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KitSchmidt.Common.Migrations
{
    public partial class AddServiceUrlToEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_CoordinatorId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "ServiceUrl",
                table: "Events",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_CoordinatorId",
                table: "Events",
                column: "CoordinatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_CoordinatorId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ServiceUrl",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_CoordinatorId",
                table: "Events",
                column: "CoordinatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
