using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoAlbum.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeColumntoComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePosted",
                table: "Comments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePosted",
                table: "Comments");
        }
    }
}
