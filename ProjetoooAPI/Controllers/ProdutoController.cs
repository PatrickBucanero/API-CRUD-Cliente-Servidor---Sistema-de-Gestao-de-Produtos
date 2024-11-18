using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoooAPI.Data;
using ProjetoooAPI.Models;
using ProjetoooAPI.ViewModels;
using System;
using System.Threading.Tasks;

namespace ProjetoooAPI.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize(Roles = "Cliente, Admin")]
        [HttpGet("products")]
        public async Task<IActionResult> GetAllAsync(
            [FromServices] AppDbContext context)
        {
            try
            {
                var products = await context.Produtos.Include(x => x.Category).ToListAsync();
                return Ok(products);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Cliente, Admin")]
        [HttpGet("product/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            int id)
        {
            try
            {
                var product = await context.Produtos.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                    return NotFound(new { message = "Produto não encontrado" });

                return Ok(product);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Cliente, Admin")]
        [HttpPost("product")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] ProdutoCreateViewModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Dados do produto inválidos" });
            }

            try
            {
                var category = await context.Categorias.FirstOrDefaultAsync(x => x.Id == model.CategoryId);
                if (category == null)
                {
                    return BadRequest(new { message = "Categoria inválida" });
                }

                var product = new Produto
                {
                    Name = model.Name,
                    Description = model.Description,
                    Value = model.Value,
                    Category = category
                };

                await context.Produtos.AddAsync(product);
                await context.SaveChangesAsync();

                return Ok(new { message = "Produto criado com sucesso!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar produto: {ex.Message}");
                return StatusCode(500, new { message = "Falha interna no servidor", error = ex.Message });
            }
        }

        [Authorize(Roles = "Cliente, Admin")]
        [HttpPut("product/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            int id,
            [FromBody] ProdutoCreateViewModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Dados do produto inválidos" });
            }

            try
            {
                var product = await context.Produtos.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                    return NotFound(new { message = "Produto não encontrado" });

                var category = await context.Categorias.FirstOrDefaultAsync(x => x.Id == model.CategoryId);
                if (category == null)
                    return BadRequest(new { message = "Categoria inválida" });

                product.Name = model.Name;
                product.Description = model.Description;
                product.Value = model.Value;
                product.Category = category;

                context.Produtos.Update(product);
                await context.SaveChangesAsync();

                return Ok(new { message = "Produto atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return StatusCode(500, new { message = "Falha interna no servidor", error = ex.Message });
            }
        }

        [Authorize(Roles = "Cliente, Admin")]
        [HttpDelete("product/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            int id)
        {
            try
            {
                var product = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                    return NotFound(new { message = "Produto não encontrado" });

                context.Produtos.Remove(product);
                await context.SaveChangesAsync();

                return Ok(new { message = "Produto excluído com sucesso!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir produto: {ex.Message}");
                return StatusCode(500, new { message = "Falha interna no servidor", error = ex.Message });
            }
        }
    }
}
