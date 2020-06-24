using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallet.Infrastructure.Data.Migrations
{
    public partial class UpdatingAssetEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssetId",
                table: "Transactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ContractAddress",
                table: "Assets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DecimalPlaces",
                table: "Assets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Assets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ContractAddress",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DecimalPlaces",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Assets");
        }
    }
}
