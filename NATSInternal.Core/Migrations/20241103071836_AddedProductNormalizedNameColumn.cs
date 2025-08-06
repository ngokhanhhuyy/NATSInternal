using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NATSInternal.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductNormalizedNameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "normalized_name",
                table: "products",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "normalized_name",
                table: "products");
        }
    }
}
