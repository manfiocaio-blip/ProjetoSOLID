using ProjetoSOLID.model;
using System;

namespace ProjetoSOLID.view
{
    public class CadastroCompra : ICadastroView<Compra>, IAtualizacaoView<Compra>
    {
        public void ExibirMensagem(string m)
        {
            Console.WriteLine(m);
        }

        public Compra ObterDados()
        {
            string cliente = ConsoleInput.LerTexto("Nome do cliente: ");

            Compra compra = new Compra(0, cliente, DateTime.Now);
            LerItens(compra);
            return compra;
        }

        public Compra ObterDadosAtualizacao(Compra atual)
        {
            Console.WriteLine("Aperte Enter para manter o valor entre [colchetes].");
            string cliente = ConsoleInput.LerTextoOpcional("Cliente", atual.Cliente);

            Compra nova = new Compra(atual.Id, cliente, atual.Data);

            // Se a pessoa não quiser refazer os carros, devolvemos a lista VAZIA.
            // O controller entende "vazia" como "mantenha os itens que já existem".
            if (ConsoleInput.LerSimNao("Deseja refazer a lista de carros desta compra?"))
                LerItens(nova);

            return nova;
        }

        // A view só sabe o ID e a quantidade digitados. Quem confere se o carro existe
        // e pega o preço real é o controller (por isso o Produto aqui é só um "marcador" com o Id).
        private static void LerItens(Compra compra)
        {
            Console.WriteLine();
            Console.WriteLine("Informe os carros da compra (digite 0 no ID para terminar).");

            while (true)
            {
                int id = ConsoleInput.LerInt("ID do carro: ", 0);
                if (id == 0)
                    break;

                int qtd = ConsoleInput.LerInt("Quantidade: ", 1);

                compra.Itens.Add(new ItemCompra(new Produto(id, "", 0, ""), qtd, 0));
            }
        }
    }
}
