using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ModernLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorBookRelationMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_BookId",
                table: "Authors");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31655194-04a0-427f-be83-a857119fc233");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "747324eb-0b76-4c7a-8e7c-429269ff7729");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7572aa3d-2f4f-491e-932e-237f2048bfb6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "804d6358-2bd5-48ea-bd1f-671a4ff5bb8d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f69a6ba4-d4a4-4355-9c59-6df092b97a0d");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorId, x.BookId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5c0ad6d3-fd08-41af-825e-da890603a648", null, "Customer", "CUSTOMER" },
                    { "72dbf581-225c-43e7-af57-bc6af8b6e0c5", null, "Staff", "STAFF" },
                    { "ba9d6e7f-fa29-4e30-94a8-948e3c47e89c", null, "SuperAdmin", "SUPERADMIN" },
                    { "c2ba1f07-4d64-4308-9163-5ea259870ed9", null, "Librarian", "LIBRARIAN" },
                    { "fcc27969-cd26-41e2-84d5-46aec6a01ff0", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BookId",
                table: "AuthorBook",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c0ad6d3-fd08-41af-825e-da890603a648");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72dbf581-225c-43e7-af57-bc6af8b6e0c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba9d6e7f-fa29-4e30-94a8-948e3c47e89c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2ba1f07-4d64-4308-9163-5ea259870ed9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fcc27969-cd26-41e2-84d5-46aec6a01ff0");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31655194-04a0-427f-be83-a857119fc233", null, "SuperAdmin", "SUPERADMIN" },
                    { "747324eb-0b76-4c7a-8e7c-429269ff7729", null, "Librarian", "LIBRARIAN" },
                    { "7572aa3d-2f4f-491e-932e-237f2048bfb6", null, "Customer", "CUSTOMER" },
                    { "804d6358-2bd5-48ea-bd1f-671a4ff5bb8d", null, "Admin", "ADMIN" },
                    { "f69a6ba4-d4a4-4355-9c59-6df092b97a0d", null, "Staff", "STAFF" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_BookId",
                table: "Authors",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId");
        }
    }
}
