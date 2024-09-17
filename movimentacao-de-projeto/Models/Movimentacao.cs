namespace movimentacao_de_projeto.Models
{
    //MODELS
    public class Movimentacao
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public required string Tipo { get; set; } // Entrada ou Saída
        public decimal Quantidade { get; set; }
        public DateTime Data { get; set; }
    }
}
