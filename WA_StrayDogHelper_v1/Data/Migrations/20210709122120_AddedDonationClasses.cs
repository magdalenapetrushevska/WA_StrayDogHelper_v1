using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WA_StrayDogHelper_v1.Data.Migrations
{
    public partial class AddedDonationClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestDonationMoneyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestDonationMoney",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(nullable: false),
                    ProblemTitle = table.Column<string>(nullable: true),
                    ProblemDescription = table.Column<string>(nullable: true),
                    AmountOfMoneyRequired = table.Column<int>(nullable: false),
                    DogName = table.Column<string>(nullable: true),
                    DogImage = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    AmountOfDonatedMoney = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDonationMoney", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestDonationMoney_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MakeDonationMoney",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountOfMoney = table.Column<int>(nullable: false),
                    RequestDonationId = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MakeDonationMoney", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MakeDonationMoney_RequestDonationMoney_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestDonationMoney",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MakeDonationMoney_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RequestDonationMoneyId",
                table: "AspNetUsers",
                column: "RequestDonationMoneyId");

            migrationBuilder.CreateIndex(
                name: "IX_MakeDonationMoney_RequestId",
                table: "MakeDonationMoney",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MakeDonationMoney_UserId",
                table: "MakeDonationMoney",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDonationMoney_UserId",
                table: "RequestDonationMoney",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RequestDonationMoney_RequestDonationMoneyId",
                table: "AspNetUsers",
                column: "RequestDonationMoneyId",
                principalTable: "RequestDonationMoney",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RequestDonationMoney_RequestDonationMoneyId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MakeDonationMoney");

            migrationBuilder.DropTable(
                name: "RequestDonationMoney");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RequestDonationMoneyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RequestDonationMoneyId",
                table: "AspNetUsers");
        }
    }
}
