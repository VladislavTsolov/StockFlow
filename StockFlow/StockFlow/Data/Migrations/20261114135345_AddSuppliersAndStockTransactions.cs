using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockFlow.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSuppliersAndStockTransactions : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.Sql(@"
                INSERT INTO Suppliers (CompanyName)
                 SELECT DISTINCT
                    CASE
                WHEN LTRIM(RTRIM(SupplierName)) = '' THEN 'Unknown supplier'
                ELSE LTRIM(RTRIM(SupplierName))
                  END
                     FROM Deliveries;
                ");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                    UPDATE d
                     SET d.SupplierId = s.Id
                 FROM Deliveries d
                     INNER JOIN Suppliers s
                   ON s.CompanyName =
                     CASE
                    WHEN LTRIM(RTRIM(d.SupplierName)) = '' THEN 'Unknown supplier'
                    ELSE LTRIM(RTRIM(d.SupplierName))
                     END;
                ");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "Deliveries",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "SupplierName",
                table: "Deliveries");

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SupplierId",
                table: "Deliveries",
                column: "SupplierId");
            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProductId",
                table: "StockTransactions",
                column: "ProductId");
            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Suppliers_SupplierId",
                table: "Deliveries",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }


        /// <inheritdoc />

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Suppliers_SupplierId",
                table: "Deliveries");

            migrationBuilder.DropTable(
                name: "StockTransactions");
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_SupplierId",
                table: "Deliveries");
            migrationBuilder.AddColumn<string>(
                name: "SupplierName",
                table: "Deliveries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql(@"
                 UPDATE d
                 SET d.SupplierName = s.CompanyName
                  FROM Deliveries d
                 INNER JOIN Suppliers s ON d.SupplierId = s.Id;
                 ");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Deliveries");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }

    }
}
