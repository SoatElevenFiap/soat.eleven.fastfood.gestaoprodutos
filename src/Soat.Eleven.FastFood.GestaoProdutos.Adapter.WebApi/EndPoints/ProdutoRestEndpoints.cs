using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Soat.Eleven.FastFood.GestaoProdutos.Application.Controllers;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Enums;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.EndPoints
{
    [ApiController]
    [Route("api/Produto")]
    public class ProdutoRestEndpoints : ControllerBase
    {
        private readonly IProdutoDataSource _produtoDataSource;
        private readonly ICategoriaProdutoDataSource _categoriaSource;
        private readonly ILogger<ProdutoRestEndpoints> _logger;

        public ProdutoRestEndpoints(IProdutoDataSource produtoDataSource, ICategoriaProdutoDataSource categoriaGateway, ILogger<ProdutoRestEndpoints> logger)
        {
            _produtoDataSource = produtoDataSource;
            _logger = logger;
            _categoriaSource = categoriaGateway;
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos(
            [FromQuery] bool incluirInativos = false,
            [FromQuery] Guid? categoriaId = null)
        {
            try
            {
                var controller = new ProdutoController(_produtoDataSource, _categoriaSource);
                var produtos = await controller.ListarProdutos(categoriaId, incluirInativos);
                return Ok(produtos);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<ProdutoDto>> GetProduto(Guid id)
        {
            var controller = new ProdutoController(_produtoDataSource, _categoriaSource);
            var produto = await controller.GetProduto(id);

            return Ok(produto);
        }

        [HttpPost]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<ActionResult<ProdutoDto>> PostProduto(CriarProdutoDto produto)
        {
            try
            {
                var controller = new ProdutoController(_produtoDataSource, _categoriaSource);

                var result = await controller.CriarProduto(produto);
                return CreatedAtAction(nameof(PostProduto), result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<IActionResult> PutProduto(Guid id, AtualizarProdutoDto produto)
        {
            try
            {
                produto.Id = id;
                var controller = new ProdutoController(_produtoDataSource, _categoriaSource);
                var produtoAtualizado = await controller.AtualizarProduto(produto);
                return Ok(produtoAtualizado);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            try
            {
                var controller = new ProdutoController(_produtoDataSource, _categoriaSource);
                await controller.DesativarProduto(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/reativar")]
        //[Authorize(PolicyRole.Administrador)]
        public async Task<IActionResult> ReativarProduto(Guid id)
        {
            try
            {
                var controller = new ProdutoController(_produtoDataSource, _categoriaSource);
                await controller.ReativarProduto(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }        
    }
}
