namespace movimentacao_de_projeto.Dto
{
    //Transferencia dos dados nna aplicação
    public class MovimentacaoCriarDto
    {
        public int IdProduto { get; set; }
        public required string Tipo { get; set; }
        public decimal Quantidade { get; set; }
    }
}
