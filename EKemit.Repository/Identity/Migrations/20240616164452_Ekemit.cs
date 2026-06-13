using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKemit.Repository.Identity.Migrations
{
    public partial class Ekemit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "372e9d05-6035-4aae-aece-2e0466197031");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "720e6fea-c71c-4a7b-9672-ebcf4b6e93ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "847b49ed-8aa7-460b-bebb-af036f5ff878");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4d786a4c-20e4-48dc-9b0a-eb3dc89ef5fa", "2", "Seller", "SELLER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "633c5943-2271-4ffb-b539-9a376d93b3b3", "1", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d16ddb5d-df1b-441f-9520-24dd16ef6a8e", "3", "Buyer", "BUYER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d786a4c-20e4-48dc-9b0a-eb3dc89ef5fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "633c5943-2271-4ffb-b539-9a376d93b3b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d16ddb5d-df1b-441f-9520-24dd16ef6a8e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "372e9d05-6035-4aae-aece-2e0466197031", "3", "Buyer", "BUYER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "720e6fea-c71c-4a7b-9672-ebcf4b6e93ee", "2", "Seller", "SELLER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "847b49ed-8aa7-460b-bebb-af036f5ff878", "1", "Admin", "ADMIN" });
        }
    }
}
