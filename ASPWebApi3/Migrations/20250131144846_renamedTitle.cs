using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPWebApi3.Migrations
{
    /// <inheritdoc />
    public partial class renamedTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TodoItems",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TodoItems",
                newName: "Title");
        }
    }
}
