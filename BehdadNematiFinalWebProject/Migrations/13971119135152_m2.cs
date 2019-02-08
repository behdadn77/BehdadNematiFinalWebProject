using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BehdadNematiFinalWebProject.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "purchaseCarts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    user_Id = table.Column<string>(nullable: true),
                    isPaid = table.Column<bool>(nullable: false),
                    pDate = table.Column<DateTime>(nullable: false),
                    exDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchaseCarts_AspNetUsers_user_Id",
                        column: x => x.user_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EnglishName = table.Column<string>(nullable: true),
                    ArabicName = table.Column<string>(nullable: true),
                    count = table.Column<int>(nullable: false),
                    price = table.Column<int>(nullable: false),
                    isAproved = table.Column<bool>(nullable: false),
                    productType_Id = table.Column<int>(nullable: false),
                    brand_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_products_brands_brand_Id",
                        column: x => x.brand_Id,
                        principalTable: "brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_products_ProductTypes_productType_Id",
                        column: x => x.productType_Id,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    img = table.Column<byte[]>(nullable: true),
                    product_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_images_products_product_Id",
                        column: x => x.product_Id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchaseCart_Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    product_Id = table.Column<int>(nullable: false),
                    purchaseCart_Id = table.Column<int>(nullable: false),
                    count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseCart_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchaseCart_Products_products_product_Id",
                        column: x => x.product_Id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_purchaseCart_Products_purchaseCarts_purchaseCart_Id",
                        column: x => x.purchaseCart_Id,
                        principalTable: "purchaseCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "specialOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    product_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_specialOffers_products_product_Id",
                        column: x => x.product_Id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "topProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    product_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_topProducts_products_product_Id",
                        column: x => x.product_Id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_images_product_Id",
                table: "images",
                column: "product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_products_brand_Id",
                table: "products",
                column: "brand_Id");

            migrationBuilder.CreateIndex(
                name: "IX_products_productType_Id",
                table: "products",
                column: "productType_Id");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseCart_Products_product_Id",
                table: "purchaseCart_Products",
                column: "product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseCart_Products_purchaseCart_Id",
                table: "purchaseCart_Products",
                column: "purchaseCart_Id");

            migrationBuilder.CreateIndex(
                name: "IX_purchaseCarts_user_Id",
                table: "purchaseCarts",
                column: "user_Id");

            migrationBuilder.CreateIndex(
                name: "IX_specialOffers_product_Id",
                table: "specialOffers",
                column: "product_Id");

            migrationBuilder.CreateIndex(
                name: "IX_topProducts_product_Id",
                table: "topProducts",
                column: "product_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "purchaseCart_Products");

            migrationBuilder.DropTable(
                name: "specialOffers");

            migrationBuilder.DropTable(
                name: "topProducts");

            migrationBuilder.DropTable(
                name: "purchaseCarts");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
