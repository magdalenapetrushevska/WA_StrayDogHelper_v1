using Microsoft.EntityFrameworkCore.Migrations;

namespace WA_StrayDogHelper_v1.Data.Migrations
{
    public partial class ChangedSomeRequestDonationMoneyProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DogImage",
                table: "RequestDonationMoney");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "RequestDonationMoney",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "RequestDonationMoney");

            migrationBuilder.AddColumn<string>(
                name: "DogImage",
                table: "RequestDonationMoney",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
