using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trembonWoW.Database.Migrations
{
    /// <inheritdoc />
    public partial class files_table_fix_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ListedFiles",
                newName: "Filename");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ListedFiles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ListedFiles");

            migrationBuilder.RenameColumn(
                name: "Filename",
                table: "ListedFiles",
                newName: "Name");
        }
    }
}
