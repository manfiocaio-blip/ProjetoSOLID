using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoSOLID.model
{
    public class Compra
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime Data { get; set; }
        public List<ItemCompra> Itens { get; set; } = new List<ItemCompra>();

        public decimal Total => Itens.Sum(i => i.Subtotal);

        public Compra(int id, string cliente, DateTime data)
        {
            Id = id;
            Cliente = cliente;
            Data = data;
        }
    }
}
