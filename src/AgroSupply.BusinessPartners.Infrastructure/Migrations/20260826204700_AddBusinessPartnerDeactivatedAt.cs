using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSupply.BusinessPartners.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessPartnerDeactivatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "BusinessPartners",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "BusinessPartners");
        }
    }
}
