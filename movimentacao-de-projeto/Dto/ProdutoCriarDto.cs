namespace movimentacao_de_projeto.Dto
{
    //Transferencia dos dados nna aplicação

    public class ProdutoCriarDto
    {
        public required string Sku { get; set; }
        public required string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataMovimentacao { get; set; }
        public decimal Saldo { get; set; }
    }
}
