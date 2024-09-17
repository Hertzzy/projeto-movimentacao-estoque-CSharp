using AutoMapper;
using Dapper;
using movimentacao_de_projeto.Dto;
using movimentacao_de_projeto.Models;
using Npgsql;

namespace movimentacao_de_projeto.Services
{
    public interface IMovimentacaoService
    {
        Task<ResponseModel<string>> CriarMovimentacao(MovimentacaoCriarDto movimentacaoDto);
        Task<ResponseModel<string>> ExcluirMovimentacao(int id);
    }

    public class MovimentacaoService : IMovimentacaoService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public MovimentacaoService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ResponseModel<string>> CriarMovimentacao(MovimentacaoCriarDto movimentacaoDto)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Verifica o saldo atual do produto                            
                var produto = await connection.QueryFirstOrDefaultAsync<Produto>("SELECT * FROM Produtos WHERE Id = @IdProduto", new { IdProduto = movimentacaoDto.IdProduto });

                if (produto == null)
                {
                    response.Mensagem = "Produto não encontrado.";
                    response.Status = false;
                    return response;
                }

                // Atualiza do saldo de acordo com a movimentação
                decimal novoSaldo = produto.Saldo;
                if (movimentacaoDto.Tipo.ToLower() == "entrada")
                {
                    novoSaldo += movimentacaoDto.Quantidade;
                }
                else if (movimentacaoDto.Tipo.ToLower() == "saida")
                {
                    if (produto.Saldo < movimentacaoDto.Quantidade)
                    {
                        response.Mensagem = "Saldo insuficiente para realizar a saída.";
                        response.Status = false;
                        return response;
                    }
                    novoSaldo -= movimentacaoDto.Quantidade;
                }

                produto.Saldo = novoSaldo;
                produto.DataMovimentacao = DateTime.Now;
                await connection.ExecuteAsync("UPDATE Produtos SET Saldo = @Saldo, DataMovimentacao = @DataMovimentacao WHERE Id = @Id", produto);

                // Registra uma nova movimentação
                var movimentacao = new Movimentacao
                {
                    IdProduto = movimentacaoDto.IdProduto,
                    Tipo = movimentacaoDto.Tipo,
                    Quantidade = movimentacaoDto.Quantidade,
                    Data = DateTime.Now
                };

                await connection.ExecuteAsync("INSERT INTO Movimentacao (IdProduto, Tipo, Quantidade, Data) VALUES (@IdProduto, @Tipo, @Quantidade, @Data)", movimentacao);

                response.Mensagem = "Movimentação registrada com sucesso!";
            }

            return response;
        }

        public async Task<ResponseModel<string>> ExcluirMovimentacao(int id)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Busca a movimentação
                var movimentacao = await connection.QueryFirstOrDefaultAsync<Movimentacao>("SELECT * FROM Movimentacao WHERE Id = @Id", new { Id = id });

                if (movimentacao == null)
                {
                    response.Mensagem = "Movimentação não encontrada.";
                    response.Status = false;
                    return response;
                }

                // Verifica se é a última movimentação do produto
                var ultimaMovimentacao = await connection.QueryFirstOrDefaultAsync<Movimentacao>("SELECT * FROM Movimentacao WHERE IdProduto = @IdProduto ORDER BY Data DESC LIMIT 1", new { movimentacao.IdProduto });

                if (ultimaMovimentacao.Id != movimentacao.Id)
                {
                    response.Mensagem = "Somente a última movimentação pode ser excluída.";
                    response.Status = false;
                    return response;
                }

                // Exclui a movimentação
                await connection.ExecuteAsync("DELETE FROM Movimentacao WHERE Id = @Id", new { Id = id });

                // Atualiza o saldo do produto
                var produto = await connection.QueryFirstOrDefaultAsync<Produto>("SELECT * FROM Produtos WHERE Id = @IdProduto", new { movimentacao.IdProduto });

                if (movimentacao.Tipo.ToLower() == "entrada")
                {
                    produto.Saldo -= movimentacao.Quantidade;
                }
                else if (movimentacao.Tipo.ToLower() == "saida")
                {
                    produto.Saldo += movimentacao.Quantidade;
                }

                // Atualiza a data de movimentação
                var movimentacaoAnterior = await connection.QueryFirstOrDefaultAsync<Movimentacao>("SELECT * FROM Movimentacao WHERE IdProduto = @IdProduto ORDER BY Data DESC LIMIT 1", new { movimentacao.IdProduto });
                produto.DataMovimentacao = movimentacaoAnterior?.Data;

                await connection.ExecuteAsync("UPDATE Produtos SET Saldo = @Saldo, DataMovimentacao = @DataMovimentacao WHERE Id = @Id", produto);

                response.Mensagem = "Movimentação excluída e saldo atualizado com sucesso!";
            }

            return response;
        }
    }
}
