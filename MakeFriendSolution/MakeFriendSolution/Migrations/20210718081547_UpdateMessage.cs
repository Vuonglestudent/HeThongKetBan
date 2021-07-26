using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakeFriendSolution.Migrations
{
    public partial class UpdateMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 18, 15, 15, 46, 923, DateTimeKind.Local).AddTicks(6806),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2021, 7, 10, 18, 2, 28, 787, DateTimeKind.Local).AddTicks(9589));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SimilariryFeatures",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 18, 15, 15, 46, 948, DateTimeKind.Local).AddTicks(6310),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2021, 7, 10, 18, 2, 28, 813, DateTimeKind.Local).AddTicks(8189));

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "HaveMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageType",
                table: "HaveMessages",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "Weight",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "Weight",
                value: 6);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "Weight",
                value: -3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "Weight",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 13,
                column: "Weight",
                value: 4);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 14,
                column: "Weight",
                value: -4);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 15,
                column: "Weight",
                value: -3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 16,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 17,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 18,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 19,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "Weight",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "Weight",
                value: 4);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 22,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 23,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 24,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 25,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 26,
                column: "Weight",
                value: 5);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 27,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 28,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 29,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 30,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 31,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 32,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 33,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 34,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 35,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 36,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 37,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 38,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 39,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 40,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 41,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 42,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 43,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 44,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 45,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 46,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 47,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 48,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 49,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 50,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 51,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 52,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 53,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 54,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 55,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 56,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 57,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 58,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 59,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 60,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 61,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 62,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 63,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 64,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 65,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 66,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 67,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 68,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 69,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 70,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 1,
                column: "WeightRate",
                value: 0.90000000000000002);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 2,
                column: "WeightRate",
                value: 0.94999999999999996);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 3,
                column: "WeightRate",
                value: 0.84999999999999998);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 4,
                column: "WeightRate",
                value: 0.82999999999999996);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 5,
                column: "WeightRate",
                value: 0.80000000000000004);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 6,
                column: "WeightRate",
                value: 0.75);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 7,
                column: "WeightRate",
                value: 0.68000000000000005);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 8,
                column: "WeightRate",
                value: 0.64000000000000001);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 9,
                column: "WeightRate",
                value: 0.87);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 10,
                column: "WeightRate",
                value: 0.88);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 11,
                column: "WeightRate",
                value: 0.80000000000000004);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 12,
                column: "WeightRate",
                value: 0.64000000000000001);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 13,
                column: "WeightRate",
                value: 0.75);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 14,
                column: "WeightRate",
                value: 0.76000000000000001);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 15,
                column: "WeightRate",
                value: 0.85999999999999999);

            migrationBuilder.UpdateData(
                table: "SimilariryFeatures",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 7, 18, 15, 15, 46, 959, DateTimeKind.Local).AddTicks(3136));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c961"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c962"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c963"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c964"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c965"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c966"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c967"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c968"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c969"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96d"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96e"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96f"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c977"),
                column: "IsInfoUpdated",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "HaveMessages");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "HaveMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 10, 18, 2, 28, 787, DateTimeKind.Local).AddTicks(9589),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 7, 18, 15, 15, 46, 923, DateTimeKind.Local).AddTicks(6806));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SimilariryFeatures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 10, 18, 2, 28, 813, DateTimeKind.Local).AddTicks(8189),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 7, 18, 15, 15, 46, 948, DateTimeKind.Local).AddTicks(6310));

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "Weight",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "Weight",
                value: 6);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "Weight",
                value: -3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 10,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 11,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 12,
                column: "Weight",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 13,
                column: "Weight",
                value: 4);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 14,
                column: "Weight",
                value: -4);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 15,
                column: "Weight",
                value: -3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 16,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 17,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 18,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 19,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 20,
                column: "Weight",
                value: 3);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 21,
                column: "Weight",
                value: 4);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 22,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 23,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 24,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 25,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 26,
                column: "Weight",
                value: 5);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 27,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 28,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 29,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 30,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 31,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 32,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 33,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 34,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 35,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 36,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 37,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 38,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 39,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 40,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 41,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 42,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 43,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 44,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 45,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 46,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 47,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 48,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 49,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 50,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 51,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 52,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 53,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 54,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 55,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 56,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 57,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 58,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 59,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 60,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 61,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 62,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 63,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 64,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 65,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 66,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 67,
                column: "Weight",
                value: -2);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 68,
                column: "Weight",
                value: -1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 69,
                column: "Weight",
                value: 1);

            migrationBuilder.UpdateData(
                table: "FeatureDetails",
                keyColumn: "Id",
                keyValue: 70,
                column: "Weight",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 1,
                column: "WeightRate",
                value: 0.90000000000000002);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 2,
                column: "WeightRate",
                value: 0.94999999999999996);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 3,
                column: "WeightRate",
                value: 0.84999999999999998);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 4,
                column: "WeightRate",
                value: 0.82999999999999996);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 5,
                column: "WeightRate",
                value: 0.80000000000000004);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 6,
                column: "WeightRate",
                value: 0.75);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 7,
                column: "WeightRate",
                value: 0.68000000000000005);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 8,
                column: "WeightRate",
                value: 0.64000000000000001);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 9,
                column: "WeightRate",
                value: 0.87);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 10,
                column: "WeightRate",
                value: 0.88);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 11,
                column: "WeightRate",
                value: 0.80000000000000004);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 12,
                column: "WeightRate",
                value: 0.64000000000000001);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 13,
                column: "WeightRate",
                value: 0.75);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 14,
                column: "WeightRate",
                value: 0.76000000000000001);

            migrationBuilder.UpdateData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 15,
                column: "WeightRate",
                value: 0.85999999999999999);

            migrationBuilder.UpdateData(
                table: "SimilariryFeatures",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2021, 7, 10, 18, 2, 28, 824, DateTimeKind.Local).AddTicks(9217));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c961"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c962"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c963"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c964"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c965"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c966"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c967"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c968"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c969"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96d"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96e"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c96f"),
                column: "IsInfoUpdated",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ec826af8-0310-48cf-8a14-da11bdb1c977"),
                column: "IsInfoUpdated",
                value: true);
        }
    }
}
