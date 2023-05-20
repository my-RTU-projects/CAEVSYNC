using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReformatTransformayionSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoolFilter",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "BoolReplacement",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "ExtraMinutesAfter",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "ExtraMinutesBefore",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "IntFilter",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "IntReplacement",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "StringFilter",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "StringReplacement",
                table: "EventTransformationSteps");

            migrationBuilder.CreateTable(
                name: "EventTransformationFilterStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IntFilter = table.Column<int>(type: "int", nullable: true),
                    StringFilter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BoolFilter = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationFilterStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationFilterStepData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationReplaceStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IntReplacement = table.Column<int>(type: "int", nullable: true),
                    StringReplacement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BoolReplacement = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationReplaceStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationReplaceStepData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationTimeExpandStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExtraMinutesBefore = table.Column<int>(type: "int", nullable: true),
                    ExtraMinutesAfter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationTimeExpandStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationTimeExpandStepData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTransformationFilterStepData");

            migrationBuilder.DropTable(
                name: "EventTransformationReplaceStepData");

            migrationBuilder.DropTable(
                name: "EventTransformationTimeExpandStepData");

            migrationBuilder.AddColumn<bool>(
                name: "BoolFilter",
                table: "EventTransformationSteps",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BoolReplacement",
                table: "EventTransformationSteps",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraMinutesAfter",
                table: "EventTransformationSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraMinutesBefore",
                table: "EventTransformationSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntFilter",
                table: "EventTransformationSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IntReplacement",
                table: "EventTransformationSteps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringFilter",
                table: "EventTransformationSteps",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringReplacement",
                table: "EventTransformationSteps",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }
    }
}
