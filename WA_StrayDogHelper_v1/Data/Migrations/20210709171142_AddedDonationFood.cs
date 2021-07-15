using Microsoft.EntityFrameworkCore.Migrations;

namespace WA_StrayDogHelper_v1.Data.Migrations
{
    public partial class AddedDonationFood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestDonationFood",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProblemTitle = table.Column<string>(nullable: true),
                    ProblemDescription = table.Column<string>(nullable: true),
                    FoodType = table.Column<int>(nullable: false),
                    AmountOfFoodRequired = table.Column<int>(nullable: false),
                    DogName = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDonationFood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestDonationFood_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestDonationFood_UserId",
                table: "RequestDonationFood",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestDonationFood");
        }
    }
}
