using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trembonWoW.Database.Migrations
{
    /// <inheritdoc />
    public partial class boost_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoostCharacterTemplate",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SetToLevel = table.Column<int>(type: "integer", nullable: false),
                    GoldToSend = table.Column<int>(type: "integer", nullable: false),
                    TeleportToHorde = table.Column<string>(type: "text", nullable: false),
                    TeleportToAlliance = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoostCharacterTemplate", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BoostedCharacters",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterID = table.Column<int>(type: "integer", nullable: false),
                    TemplateID = table.Column<Guid>(type: "uuid", nullable: false),
                    BoostedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoostedCharacters", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BoostedCharacters_BoostCharacterTemplate_TemplateID",
                        column: x => x.TemplateID,
                        principalTable: "BoostCharacterTemplate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoostedCharacters_TemplateID",
                table: "BoostedCharacters",
                column: "TemplateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoostedCharacters");

            migrationBuilder.DropTable(
                name: "BoostCharacterTemplate");
        }
    }
}
