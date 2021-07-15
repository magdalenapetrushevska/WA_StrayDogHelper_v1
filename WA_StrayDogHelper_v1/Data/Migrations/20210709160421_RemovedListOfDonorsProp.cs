using Microsoft.EntityFrameworkCore.Migrations;

namespace WA_StrayDogHelper_v1.Data.Migrations
{
    public partial class RemovedListOfDonorsProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RequestDonationMoney_RequestDonationMoneyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RequestDonationMoneyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RequestDonationMoneyId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestDonationMoneyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RequestDonationMoneyId",
                table: "AspNetUsers",
                column: "RequestDonationMoneyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RequestDonationMoney_RequestDonationMoneyId",
                table: "AspNetUsers",
                column: "RequestDonationMoneyId",
                principalTable: "RequestDonationMoney",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
