namespace ProjetoSOLID.model
{
    /// <summary>Um carro à venda (no projeto de aula chamava-se Produto).</summary>
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }

        public Produto(int id, string nome, decimal preco, string descricao)
        {
            Id = id;
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
        }
    }
}
