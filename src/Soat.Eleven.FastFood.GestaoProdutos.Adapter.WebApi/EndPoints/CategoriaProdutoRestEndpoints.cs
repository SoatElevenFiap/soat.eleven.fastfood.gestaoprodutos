using Microsoft.AspNetCore.Mvc;
using Soat.Eleven.FastFood.GestaoProdutos.Application.Controllers;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.EndPoints
{
    [ApiController]
    [Route("api/Categoria")]
    public class CategoriaProdutoRestEndpoints : ControllerBase
    {
        private readonly ICategoriaProdutoDataSource _categoriaDataSource;

        public CategoriaProdutoRestEndpoints(ICategoriaProdutoDataSource categoriaDataSource)
        {
            _categoriaDataSource = categoriaDataSource;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaProdutoDto>>> GetCategorias()
        {
            var controller = new CategoriaProdutoController(_categoriaDataSource);
            var categorias = await controller.ListarCategorias(incluirInativos: true);
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<CategoriaProdutoDto>> GetCategoria(Guid id)
        {
            var controller = new CategoriaProdutoController(_categoriaDataSource);
            var categoria = await controller.GetCategoriaPorId(id);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<ActionResult<CategoriaProdutoDto>> PostCategoria(CriarCategoriaDto categoria)
        {
            var controller = new CategoriaProdutoController(_categoriaDataSource);
            var categoriaCriada = await controller.CriarCategoria(categoria);
            return CreatedAtAction(nameof(PostCategoria), new { id = categoriaCriada.Id }, categoriaCriada);
        }

        [HttpPut("{id}")]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<IActionResult> PutCategoria(Guid id, AtualizarCategoriaDto categoria)
        {
            try
            {
                var controller = new CategoriaProdutoController(_categoriaDataSource);
                var categoriaAtualizada = await controller.AtualizarCategoria(id, categoria);
                return Ok(categoriaAtualizada);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<IActionResult> DeleteCategoria(Guid id)
        {
            try
            {
                var controller = new CategoriaProdutoController(_categoriaDataSource);
                await controller.DesativarCategoria(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/reativar")]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<IActionResult> ReativarCategoria(Guid id)
        {
            try
            {
                var controller = new CategoriaProdutoController(_categoriaDataSource);
                await controller.ReativarCategoria(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}