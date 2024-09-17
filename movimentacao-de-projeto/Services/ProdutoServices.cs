using AutoMapper;
using Dapper;
using movimentacao_de_projeto.Dto;
using movimentacao_de_projeto.Models;
using Npgsql;

namespace movimentacao_de_projeto.Services
{
    public class ProdutoServices : IProdutoInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProdutoServices(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<List<ProdutoListarDto>>> BuscarProdutos()
        {
            ResponseModel<List<ProdutoListarDto>> response = new ResponseModel<List<ProdutoListarDto>>();

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var produtosBanco = await connection.QueryAsync<Produto>("SELECT * FROM Produtos");

                if (produtosBanco.Count() == 0)
                {
                    response.Mensagem = "Nenhum produto localizado!";
                    response.Status = false;
                    return response;
                }

                var produtosMapeados = _mapper.Map<List<ProdutoListarDto>>(produtosBanco);

                response.Dados = produtosMapeados;
                response.Mensagem = "Produtos localizados com sucesso";
            }

            return response;
        }

        public async Task<ResponseModel<List<ProdutoListarDto>>> CriarProduto(ProdutoCriarDto produtoCriarDto)
        {
            ResponseModel<List<ProdutoListarDto>> response = new ResponseModel<List<ProdutoListarDto>>();

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {                                                   //Querie para criar um novo produto da tabela Produtos
                var produtoBanco = await connection.ExecuteAsync("INSERT INTO Produtos (Sku, Descricao, DataCadastro, DataMovimentacao, Saldo) " +
                    "VALUES (@Sku, @Descricao, @DataCadastro, @DataMovimentacao, @Saldo)", produtoCriarDto);

                if (produtoBanco == 0)
                {
                    response.Mensagem = "Ocorreu um erro ao realizar o registro!";
                    response.Status = false;
                    return response;
                }

                var produtos = await ListarProdutos(connection);

                var produtosMapeados = _mapper.Map<List<ProdutoListarDto>>(produtos);

                response.Dados = produtosMapeados;
                response.Mensagem = "Produtos listados com sucesso!";
            }

            return response;
        }

        private static async Task<IEnumerable<Produto>> ListarProdutos(NpgsqlConnection connection)
        {   //Lista os todos o produtos da tabela
            return await connection.QueryAsync<Produto>("SELECT * FROM Produtos");
        }
    }
}
