using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKemit.Repository.Data.Migrations
{
    public partial class AddGovernratesPictureUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "Governrates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "Governrates");
        }
    }
}
