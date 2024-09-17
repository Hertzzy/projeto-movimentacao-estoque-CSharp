namespace movimentacao_de_projeto.Dto
{
    //Transferencia dos dados nna aplicação
    public class MovimentacaoListarDto
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public required string Tipo { get; set; }
        public decimal Quantidade { get; set; }
        public DateTime Data { get; set; }
    }
}
