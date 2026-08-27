using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSupply.BusinessPartners.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_BusinessPartners_BusinessPartnerId",
                table: "PhoneNumbers");

            migrationBuilder.CreateTable(
                name: "BusinessRelationships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierBusinessPartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuyerBusinessPartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeactivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessRelationships_BusinessPartners_BuyerBusinessPartnerId",
                        column: x => x.BuyerBusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessRelationships_BusinessPartners_SupplierBusinessPartnerId",
                        column: x => x.SupplierBusinessPartnerId,
                        principalTable: "BusinessPartners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRelationships_BuyerBusinessPartnerId",
                table: "BusinessRelationships",
                column: "BuyerBusinessPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessRelationships_SupplierBusinessPartnerId",
                table: "BusinessRelationships",
                column: "SupplierBusinessPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_BusinessPartners_BusinessPartnerId",
                table: "PhoneNumbers",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_BusinessPartners_BusinessPartnerId",
                table: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "BusinessRelationships");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_BusinessPartners_BusinessPartnerId",
                table: "PhoneNumbers",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
