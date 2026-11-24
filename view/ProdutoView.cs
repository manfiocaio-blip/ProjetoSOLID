using ProjetoSOLID.model;
using System;

namespace ProjetoSOLID.view
{
    public class ProdutoView : IProdutoView
    {
        public void MostrarProduto(Produto produto)
        {
            Console.WriteLine($"ID: {produto.Id}");
            Console.WriteLine($"Carro: {produto.Nome}");
            Console.WriteLine($"Preço: {produto.Preco:C}");
            Console.WriteLine($"Descrição: {produto.Descricao}");
            Console.WriteLine("----------------------------------");
        }
    }
}
