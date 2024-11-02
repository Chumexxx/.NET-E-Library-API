using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ModernLibrary.Migrations
{
    /// <inheritdoc />
    public partial class DateOfBirthAndHomeAddressToAppUserModelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "0901f366-071b-4cd5-9490-dd3b6a61a852", null, "Customer", "CUSTOMER" },
                    { "3bedc24b-6093-41b0-917e-08d45ae6433f", null, "Librarian", "LIBRARIAN" },
                    { "cddffe8e-8a45-427c-802e-1a4feb2a8bd1", null, "Admin", "ADMIN" },
                    { "fa3df17b-4889-4b80-8451-548b9437407d", null, "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0901f366-071b-4cd5-9490-dd3b6a61a852");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bedc24b-6093-41b0-917e-08d45ae6433f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cddffe8e-8a45-427c-802e-1a4feb2a8bd1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa3df17b-4889-4b80-8451-548b9437407d");

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
    }
}
