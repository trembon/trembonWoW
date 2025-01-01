using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trembonWoW.Database.Migrations
{
    /// <inheritdoc />
    public partial class account_to_boostchars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "BoostedCharacters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "BoostedCharacters");
        }
    }
}
