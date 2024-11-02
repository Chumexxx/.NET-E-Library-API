using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ModernLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCancelledAndCancelDateToBorrowingRecordModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ae23fb5-7739-4926-9efd-33ae929afb92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0bc52b4c-e95b-4c4d-b887-13a36849b357");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5c0096e-afda-4739-9a7d-1350b25fec4b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e30a30f4-7986-4550-9a40-9c2e6e31231d");

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDate",
                table: "Borrowing Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Borrowing Record",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "38a4e626-22cf-4ff7-b0ee-1c4e536449b6", null, "Customer", "CUSTOMER" },
                    { "591d3416-a654-4d34-99a3-1fb668cd2eb4", null, "SuperAdmin", "SUPERADMIN" },
                    { "8bb1be22-1812-4bfc-bd99-55f1cfc4d606", null, "Staff", "STAFF" },
                    { "a33a1c52-2bc1-4a49-bb08-5484addbbd1a", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38a4e626-22cf-4ff7-b0ee-1c4e536449b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "591d3416-a654-4d34-99a3-1fb668cd2eb4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bb1be22-1812-4bfc-bd99-55f1cfc4d606");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a33a1c52-2bc1-4a49-bb08-5484addbbd1a");

            migrationBuilder.DropColumn(
                name: "CancelDate",
                table: "Borrowing Record");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Borrowing Record");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ae23fb5-7739-4926-9efd-33ae929afb92", null, "Admin", "ADMIN" },
                    { "0bc52b4c-e95b-4c4d-b887-13a36849b357", null, "Customer", "CUSTOMER" },
                    { "d5c0096e-afda-4739-9a7d-1350b25fec4b", null, "Staff", "STAFF" },
                    { "e30a30f4-7986-4550-9a40-9c2e6e31231d", null, "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
