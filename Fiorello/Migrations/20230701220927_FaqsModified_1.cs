using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiorello.Migrations
{
    /// <inheritdoc />
    public partial class FaqsModified_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Faqs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Faqs");
        }
    }
}
