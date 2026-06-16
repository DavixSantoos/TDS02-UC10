namespace ControleEstoque.API.Models
{
    public class FormaPagamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Ativo { get; set; }
    }
}
