using ProjetoSOLID.model;
using System;

namespace ProjetoSOLID.view
{
    public class CompraView : ICompraView
    {
        public void MostrarCompra(Compra compra)
        {
            Console.WriteLine($"COMPRA Nº {compra.Id}");
            Console.WriteLine($"Cliente: {compra.Cliente}");
            Console.WriteLine($"Data: {compra.Data:dd/MM/yyyy HH:mm}");
            Console.WriteLine("Carros:");

            foreach (ItemCompra item in compra.Itens)
            {
                Console.WriteLine(
                    $"  - [{item.Produto.Id}] {item.Produto.Nome} | " +
                    $"{item.Quantidade} x {item.PrecoUnitario:C} = {item.Subtotal:C}");
            }

            Console.WriteLine($"TOTAL: {compra.Total:C}");
            Console.WriteLine("==================================");
        }
    }
}
