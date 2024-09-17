using movimentacao_de_projeto.Dto;
using movimentacao_de_projeto.Models;

namespace movimentacao_de_projeto.Services
{
    public interface IProdutoInterface
    {
        // Method | Tipo de retorno | Nome do metodo
        Task<ResponseModel<List<ProdutoListarDto>>> BuscarProdutos();
        Task<ResponseModel<List<ProdutoListarDto>>> CriarProduto(ProdutoCriarDto produtoCriarDto);

    }
}
