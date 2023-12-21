using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspWebProgram.Migrations
{
    public partial class AddHastaSifreToHasta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HastaSifre",
                table: "Hastalar",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HastaSifre",
                table: "Hastalar");
        }
    }
}
