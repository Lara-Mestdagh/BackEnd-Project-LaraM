using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DnD_CO.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DMCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Race = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentHP = table.Column<int>(type: "int", nullable: true),
                    MaxHP = table.Column<int>(type: "int", nullable: false),
                    TempHP = table.Column<int>(type: "int", nullable: false),
                    Conditions = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WalkingSpeed = table.Column<int>(type: "int", nullable: false),
                    FlyingSpeed = table.Column<int>(type: "int", nullable: true),
                    SwimmingSpeed = table.Column<int>(type: "int", nullable: true),
                    DarkvisionRange = table.Column<int>(type: "int", nullable: true),
                    LegendaryActions = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecialAbilities = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DMCharacters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Race = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentHP = table.Column<int>(type: "int", nullable: false),
                    MaxHP = table.Column<int>(type: "int", nullable: false),
                    TempHP = table.Column<int>(type: "int", nullable: false),
                    Conditions = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WalkingSpeed = table.Column<int>(type: "int", nullable: false),
                    FlyingSpeed = table.Column<int>(type: "int", nullable: true),
                    SwimmingSpeed = table.Column<int>(type: "int", nullable: true),
                    DarkvisionRange = table.Column<int>(type: "int", nullable: true),
                    IsAlive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PlayerName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharacters", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SharedInventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedInventory", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CharacterClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerCharacterId = table.Column<int>(type: "int", nullable: false),
                    ClassName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    DMCharacterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterClass_DMCharacters_DMCharacterId",
                        column: x => x.DMCharacterId,
                        principalTable: "DMCharacters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CharacterClass_PlayerCharacters_PlayerCharacterId",
                        column: x => x.PlayerCharacterId,
                        principalTable: "PlayerCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerCharacterId = table.Column<int>(type: "int", nullable: true),
                    DMCharacterId = table.Column<int>(type: "int", nullable: true),
                    SharedInventoryId = table.Column<int>(type: "int", nullable: true),
                    ItemName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    WeightUnit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_DMCharacters_DMCharacterId",
                        column: x => x.DMCharacterId,
                        principalTable: "DMCharacters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItems_PlayerCharacters_PlayerCharacterId",
                        column: x => x.PlayerCharacterId,
                        principalTable: "PlayerCharacters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InventoryItems_SharedInventory_SharedInventoryId",
                        column: x => x.SharedInventoryId,
                        principalTable: "SharedInventory",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "SharedInventory",
                column: "Id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClass_DMCharacterId",
                table: "CharacterClass",
                column: "DMCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClass_PlayerCharacterId",
                table: "CharacterClass",
                column: "PlayerCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_DMCharacterId",
                table: "InventoryItems",
                column: "DMCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_PlayerCharacterId",
                table: "InventoryItems",
                column: "PlayerCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SharedInventoryId",
                table: "InventoryItems",
                column: "SharedInventoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClass");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "DMCharacters");

            migrationBuilder.DropTable(
                name: "PlayerCharacters");

            migrationBuilder.DropTable(
                name: "SharedInventory");
        }
    }
}
