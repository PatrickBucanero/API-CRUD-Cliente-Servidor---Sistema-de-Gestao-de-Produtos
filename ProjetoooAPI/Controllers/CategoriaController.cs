using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoooAPI.Data;
using ProjetoooAPI.Models;
using ProjetoooAPI.ViewModels;
using static ProjetoooAPI.ViewModels.CategoriaViewModel;

namespace ProjetoooAPI.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [Authorize(Roles = "Cliente,Admin")]
        [HttpPost("category")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CategoriaCreateViewModel model)
        {
            try
            {
                var category = new Categoria
                {
                    Name = model.Name
                };

                await context.Categorias.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"/categoria/{category.Id}", category);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] AppDbContext context)
        {
            try
            {
                var categories = await context.Categorias.ToListAsync();
                return Ok(categories);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("category/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context,
            int id)
        {
            try
            {
                var category = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new { message = "Categoria não encontrada" });

                return Ok(category);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("category/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            int id,
            [FromBody] CategoriaCreateViewModel model)
        {
            try
            {
                var category = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new { message = "Categoria não encontrada" });

                category.Name = model.Name;

                context.Categorias.Update(category);
                await context.SaveChangesAsync();

                return Ok(new { message = "Categoria atualizada com sucesso!" });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("category/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
    [FromServices] AppDbContext context,
    int id)
        {
            try
            {
                var category = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound(new { message = "Categoria não encontrada" });
                }

                context.Categorias.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new { message = "Categoria excluída com sucesso!" });
            }
            catch (DbUpdateException dbEx)
            {
                // Captura e loga exceções de banco de dados, como restrições de integridade
                // _logger.LogError(dbEx, $"Erro ao excluir a categoria com ID {id}.");
                return StatusCode(500, new { message = "Erro de integridade ao excluir a categoria. Verifique se não há dependências." });
            }
            catch (Exception ex)
            {
                // Loga outras exceções inesperadas
                // _logger.LogError(ex, "Erro desconhecido ao excluir categoria.");
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }
    }
}
