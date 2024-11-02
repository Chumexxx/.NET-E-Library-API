using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ModernLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStaffRoleFromSeedRoleAndAddLibrarianToSeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5867fc43-38a5-493c-ba42-95d6b8b871f3", null, "Customer", "CUSTOMER" },
                    { "87d3c7bd-2a14-43bf-b4d5-93b83474ea89", null, "Admin", "ADMIN" },
                    { "9f1c2ee7-aafd-4a8d-8f53-1a74bfa4dfdc", null, "Librarian", "LIBRARIAN" },
                    { "f54e4319-ec94-4ace-aab1-7a58de45b2c0", null, "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5867fc43-38a5-493c-ba42-95d6b8b871f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87d3c7bd-2a14-43bf-b4d5-93b83474ea89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f1c2ee7-aafd-4a8d-8f53-1a74bfa4dfdc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f54e4319-ec94-4ace-aab1-7a58de45b2c0");

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
    }
}
