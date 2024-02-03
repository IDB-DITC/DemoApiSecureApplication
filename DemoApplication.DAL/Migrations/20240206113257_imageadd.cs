using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApplication.DAL.Migrations
{
    /// <inheritdoc />
    public partial class imageadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Salesmen",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Salesmen");
        }
    }
}
