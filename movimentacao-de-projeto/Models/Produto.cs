namespace movimentacao_de_projeto.Models
{
    //Models
    public class Produto
    {
        public int Id { get; set; }
        public required string Sku { get; set; }
        public required string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataMovimentacao { get; set; }
        public decimal Saldo { get; set; }

    }
}
