using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallet.Infrastructure.Data.Migrations
{
    public partial class UpdatingAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicAddress",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicAddress",
                table: "Addresses");
        }
    }
}
