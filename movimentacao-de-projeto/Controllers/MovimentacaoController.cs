using Microsoft.AspNetCore.Mvc;
using movimentacao_de_projeto.Dto;
using movimentacao_de_projeto.Services;

namespace movimentacao_de_projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMovimentacaoService _movimentacaoService;

        public MovimentacaoController(IMovimentacaoService movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
        }
        //METHODS
        // Criar Movimentação
        [HttpPost]
        public async Task<IActionResult> CriarMovimentacao(MovimentacaoCriarDto movimentacaoDto)
        {
            var resultado = await _movimentacaoService.CriarMovimentacao(movimentacaoDto);

            if (!resultado.Status)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }

        // Excluir Movimentação
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirMovimentacao(int id)
        {
            var resultado = await _movimentacaoService.ExcluirMovimentacao(id);

            if (!resultado.Status)
            {
                return BadRequest(resultado);
            }

            return Ok(resultado);
        }
    }
}
