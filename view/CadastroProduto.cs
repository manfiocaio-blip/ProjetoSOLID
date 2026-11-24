using ProjetoSOLID.model;
using System;

namespace ProjetoSOLID.view
{
    public class CadastroProduto : ICadastroView<Produto>, IAtualizacaoView<Produto>
    {
        public void ExibirMensagem(string m)
        {
            Console.WriteLine(m);
        }

        public Produto ObterDados()
        {
            string nome = ConsoleInput.LerTexto("Carro (marca e modelo): ");
            decimal preco = ConsoleInput.LerDecimal("Preço: ");
            string desc = ConsoleInput.LerTexto("Descrição (ano, cor, km...): ");

            // Id 0: quem define o Id verdadeiro é o banco de dados.
            return new Produto(0, nome, preco, desc);
        }

        public Produto ObterDadosAtualizacao(Produto atual)
        {
            Console.WriteLine("Aperte Enter para manter o valor entre [colchetes].");

            string nome = ConsoleInput.LerTextoOpcional("Carro", atual.Nome);
            decimal preco = ConsoleInput.LerDecimalOpcional("Preço", atual.Preco);
            string desc = ConsoleInput.LerTextoOpcional("Descrição", atual.Descricao);

            return new Produto(atual.Id, nome, preco, desc);
        }
    }
}
