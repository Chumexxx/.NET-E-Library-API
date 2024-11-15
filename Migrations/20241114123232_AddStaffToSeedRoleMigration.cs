using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ModernLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffToSeedRoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CancelDate",
                table: "Borrowing Record",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20b9d908-1823-45cb-b6b2-85ec80749ad9", null, "Librarian", "LIBRARIAN" },
                    { "31c62eac-3daa-4f5e-8b17-fae2b44ba84d", null, "SuperAdmin", "SUPERADMIN" },
                    { "4f7c625b-4519-4432-8fda-068a2fd8c61c", null, "Staff", "STAFF" },
                    { "722a77d0-b915-4771-963c-9b79e9e85e6e", null, "Customer", "CUSTOMER" },
                    { "c5bbf02f-8f30-41bf-9281-64bf910f2969", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20b9d908-1823-45cb-b6b2-85ec80749ad9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31c62eac-3daa-4f5e-8b17-fae2b44ba84d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f7c625b-4519-4432-8fda-068a2fd8c61c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "722a77d0-b915-4771-963c-9b79e9e85e6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5bbf02f-8f30-41bf-9281-64bf910f2969");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CancelDate",
                table: "Borrowing Record",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorName",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId");
        }
    }
}
