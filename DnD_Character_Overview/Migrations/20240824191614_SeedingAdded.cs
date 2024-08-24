using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DnD_CO.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DMCharacters",
                columns: new[] { "Id", "ArmorClass", "Charisma", "Conditions", "Constitution", "CurrentHP", "DarkvisionRange", "Description", "Dexterity", "FlyingSpeed", "HasOver20Stats", "ImagePath", "Intelligence", "IsAlive", "KnownLanguages", "LegendaryActions", "MaxHP", "Name", "Race", "Resistances", "SpecialAbilities", "Strength", "SwimmingSpeed", "TempHP", "WalkingSpeed", "Weaknesses", "Wisdom" },
                values: new object[] { 1, 17, 12, "", 16, 100, null, null, 14, null, false, null, 8, true, "Common,Orc", "Legendary Slash, Intimidating Roar", 100, "Zugthrok the Mighty", "Orc", "Bludgeoning,Poison", "Darkvision, Fury of Blows", 18, null, 0, 30, "Psychic,Fire", 10 });

            migrationBuilder.InsertData(
                table: "PlayerCharacters",
                columns: new[] { "Id", "ArmorClass", "Charisma", "Conditions", "Constitution", "CurrentHP", "DarkvisionRange", "Dexterity", "FlyingSpeed", "HasOver20Stats", "ImagePath", "Intelligence", "IsAlive", "KnownLanguages", "MaxHP", "Name", "PlayerName", "Race", "Resistances", "Strength", "SwimmingSpeed", "TempHP", "WalkingSpeed", "Weaknesses", "Wisdom" },
                values: new object[,]
                {
                    { 1, 16, 16, "", 14, 35, null, 14, null, false, null, 12, true, "Common", 40, "Arthas", "John", "Human", "Bludgeoning,Cold", 16, null, 0, 30, "Fire,Lightning", 10 },
                    { 2, 14, 18, "", 12, 25, 60, 16, null, false, null, 14, true, "Common,Elvish", 30, "Luna", "Jane", "Half-Elf", "Radiant,Thunder", 10, 30, 0, 30, "Necrotic,Poison", 14 }
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "DMCharacterId", "ItemName", "PlayerCharacterId", "Quantity", "SharedInventoryId", "Weight", "WeightUnit" },
                values: new object[,]
                {
                    { 1, null, "Sword", 1, 1, null, 3.0m, "lb" },
                    { 2, null, "Shield", 2, 1, null, 6.0m, "lb" },
                    { 3, 1, "Health Potion", null, 5, null, 0.5m, "lb" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DMCharacters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlayerCharacters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlayerCharacters",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
