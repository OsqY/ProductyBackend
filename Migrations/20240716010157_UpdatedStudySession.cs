﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Producty.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedStudySession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastStudyDate",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastStudyDate",
                table: "Users");
        }
    }
}
