using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoooAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "Produtos",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                newName: "IX_Produtos_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CategoryId",
                table: "Produtos",
                column: "CategoryId",
                principalTable: "Categorias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_CategoryId",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Produtos",
                newName: "CategoriaId");

            migrationBuilder.RenameIndex(
                name: "IX_Produtos_CategoryId",
                table: "Produtos",
                newName: "IX_Produtos_CategoriaId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Produtos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CategoriaId",
                table: "Produtos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id");
        }
    }
}
