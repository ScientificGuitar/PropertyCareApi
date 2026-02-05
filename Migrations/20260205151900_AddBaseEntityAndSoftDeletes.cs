using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyCareApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityAndSoftDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "properties",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "properties",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "maintenance_requests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "maintenance_requests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_DeletedAt",
                table: "users",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_properties_DeletedAt",
                table: "properties",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_requests_DeletedAt",
                table: "maintenance_requests",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_DeletedAt",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_properties_DeletedAt",
                table: "properties");

            migrationBuilder.DropIndex(
                name: "IX_maintenance_requests_DeletedAt",
                table: "maintenance_requests");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "maintenance_requests");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "maintenance_requests");
        }
    }
}
