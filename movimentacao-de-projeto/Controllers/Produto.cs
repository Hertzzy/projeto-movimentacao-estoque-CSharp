using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movimentacao_de_projeto.Dto;
using movimentacao_de_projeto.Services;

namespace movimentacao_de_projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Produto : ControllerBase
    {
        private readonly IProdutoInterface _produtoInterface;
        public Produto(IProdutoInterface produtoInterface)
        {
            _produtoInterface = produtoInterface;
        }

        //METHODS
        //Criar um produto
        [HttpPost]
        public async Task<IActionResult> CriarProduto(ProdutoCriarDto produtoCriarDto)
        {
            var produtos = await _produtoInterface.CriarProduto(produtoCriarDto);

            if (produtos.Status == false)
            {
                return BadRequest(produtos);
            }

            return Ok(produtos);
        }

        //METHODS
        // Listar produtos
        [HttpGet]
        public async Task<IActionResult> BuscarProdutos()
        {
            var produtos = await _produtoInterface.BuscarProdutos();

            if(produtos.Status == false)
            {
                return NotFound(produtos);
            }

            return Ok(produtos);
        }
    }
}
