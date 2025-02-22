using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NetLib.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Book_Id", "Author", "Description", "ISBN", "Image", "IsAvailable", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Andrew Hunt and David Thomas", null, "978-0201616224", null, true, new DateTime(2021, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Pragmatic Programmer" },
                    { 2, "Robert C. Martin", null, "978-0132350884", null, true, new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Design Pattern using C#" },
                    { 3, "Pranaya Kumar Rout", null, "978-0451616235", null, true, new DateTime(2022, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mastering ASP.NET Core" },
                    { 4, "Rakesh Kumat", null, "978-4562350123", null, true, new DateTime(2020, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SQL Server with DBA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Book_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Book_Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Book_Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Book_Id",
                keyValue: 4);
        }
    }
}
