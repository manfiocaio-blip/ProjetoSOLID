namespace ProjetoSOLID.model
{
    /// <summary>
    /// Linha da compra: liga uma Compra a um Produto (relação n:n).
    /// Guarda o preço do momento da compra, para o histórico não mudar se o preço do carro mudar.
    /// </summary>
    public class ItemCompra
    {
        public Produto Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public decimal Subtotal => PrecoUnitario * Quantidade;

        public ItemCompra(Produto produto, int quantidade, decimal precoUnitario)
        {
            Produto = produto;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }
    }
}
