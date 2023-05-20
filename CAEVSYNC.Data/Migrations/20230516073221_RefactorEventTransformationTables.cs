using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorEventTransformationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTransformationFilterStepData");

            migrationBuilder.DropTable(
                name: "EventTransformationReplaceStepData");

            migrationBuilder.CreateTable(
                name: "EventTransformationBoolFilterData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BoolFilter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationBoolFilterData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationBoolFilterData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationBoolReplaceStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BoolReplacement = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationBoolReplaceStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationBoolReplaceStepData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationDateTimeFilterData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FromDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationDateTimeFilterData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationDateTimeFilterData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationIntFilterStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IntFilter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationIntFilterStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationIntFilterStepData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationIntReplaceStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IntReplacement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationIntReplaceStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationIntReplaceStepData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationStringFilterData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StringFilter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationStringFilterData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationStringFilterData_EventTransformationSteps_Id",
                        column: x => x.Id,
                        principalTable: "EventTransformationSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTransformationStringReplaceStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StringReplacement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationStringReplaceStepData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationStringReplaceStepData_EventTransformationSteps_Id",
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
                name: "EventTransformationBoolFilterData");

            migrationBuilder.DropTable(
                name: "EventTransformationBoolReplaceStepData");

            migrationBuilder.DropTable(
                name: "EventTransformationDateTimeFilterData");

            migrationBuilder.DropTable(
                name: "EventTransformationIntFilterStepData");

            migrationBuilder.DropTable(
                name: "EventTransformationIntReplaceStepData");

            migrationBuilder.DropTable(
                name: "EventTransformationStringFilterData");

            migrationBuilder.DropTable(
                name: "EventTransformationStringReplaceStepData");

            migrationBuilder.CreateTable(
                name: "EventTransformationFilterStepData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BoolFilter = table.Column<bool>(type: "bit", nullable: true),
                    FromDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IntFilter = table.Column<int>(type: "int", nullable: true),
                    StringFilter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ToDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    BoolReplacement = table.Column<bool>(type: "bit", nullable: true),
                    IntReplacement = table.Column<int>(type: "int", nullable: true),
                    StringReplacement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
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
        }
    }
}
