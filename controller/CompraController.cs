using ProjetoSOLID.dados;
using ProjetoSOLID.model;
using ProjetoSOLID.view;
using System.Collections.Generic;

namespace ProjetoSOLID.controller
{
    public class CompraController
    {
        private readonly IRepository<Compra> repository;
        private readonly IRepository<Produto> produtoRepository;
        private readonly ICompraView view;
        private readonly IProdutoView produtoView;
        private readonly ICadastroView<Compra> cadastroView;
        private readonly IAtualizacaoView<Compra> atualizacaoView;

        public CompraController(
            ICompraView view,
            IRepository<Compra> repository,
            IRepository<Produto> produtoRepository,
            IProdutoView produtoView,
            ICadastroView<Compra> cadastroView,
            IAtualizacaoView<Compra> atualizacaoView)
        {
            this.view = view;
            this.repository = repository;
            this.produtoRepository = produtoRepository;
            this.produtoView = produtoView;
            this.cadastroView = cadastroView;
            this.atualizacaoView = atualizacaoView;
        }

        public void Cadastrar()
        {
            List<Produto> disponiveis = produtoRepository.ListarTodos();

            if (disponiveis.Count == 0)
            {
                cadastroView.ExibirMensagem("Cadastre pelo menos um carro antes de registrar uma compra.");
                return;
            }

            cadastroView.ExibirMensagem("Carros disponíveis:");
            foreach (Produto p in disponiveis)
                produtoView.MostrarProduto(p);

            Compra compra = cadastroView.ObterDados();

            if (!PrepararItens(compra))
                return;

            repository.Adicionar(compra);

            cadastroView.ExibirMensagem(
                $"Compra registrada com sucesso! (Nº {compra.Id})"
            );
        }

        public void Listar()
        {
            List<Compra> compras = repository.ListarTodos();

            if (compras.Count == 0)
            {
                cadastroView.ExibirMensagem("Nenhuma compra registrada.");
                return;
            }

            foreach (Compra compra in compras)
                view.MostrarCompra(compra);
        }

        public Compra Buscar(int id)
        {
            return repository.BuscarPorId(id);
        }

        public void Exibir(int id)
        {
            Compra compra = Buscar(id);

            if (compra == null)
                cadastroView.ExibirMensagem("Compra não encontrada.");
            else
                view.MostrarCompra(compra);
        }

        public void Atualizar(int id)
        {
            Compra existente = Buscar(id);

            if (existente == null)
            {
                cadastroView.ExibirMensagem("Compra não encontrada.");
                return;
            }

            view.MostrarCompra(existente);

            Compra atualizada = atualizacaoView.ObterDadosAtualizacao(existente);

            if (atualizada.Itens.Count == 0)
            {
                // Lista vazia = a pessoa não quis mexer nos carros: mantém os que já existiam.
                atualizada.Itens = existente.Itens;
            }
            else if (!PrepararItens(atualizada))
            {
                return;
            }

            repository.Atualizar(atualizada);

            cadastroView.ExibirMensagem("Compra atualizada com sucesso!");
        }

        public void Remover(int id)
        {
            if (Buscar(id) == null)
            {
                cadastroView.ExibirMensagem("Compra não encontrada.");
                return;
            }

            repository.Remover(id);
            cadastroView.ExibirMensagem("Compra excluída com sucesso!");
        }

        /// <summary>
        /// Confere se cada carro existe, junta linhas repetidas do mesmo carro
        /// e grava o preço atual do carro em cada item.
        /// </summary>
        private bool PrepararItens(Compra compra)
        {
            if (compra.Itens.Count == 0)
            {
                cadastroView.ExibirMensagem("A compra precisa ter pelo menos um carro.");
                return false;
            }

            Dictionary<int, int> quantidades = new Dictionary<int, int>();
            foreach (ItemCompra item in compra.Itens)
            {
                int idProduto = item.Produto.Id;
                quantidades.TryGetValue(idProduto, out int atual);
                quantidades[idProduto] = atual + item.Quantidade;
            }

            List<ItemCompra> itensValidos = new List<ItemCompra>();
            foreach (KeyValuePair<int, int> par in quantidades)
            {
                Produto produto = produtoRepository.BuscarPorId(par.Key);

                if (produto == null)
                {
                    cadastroView.ExibirMensagem($"O carro de ID {par.Key} não existe. Operação cancelada.");
                    return false;
                }

                itensValidos.Add(new ItemCompra(produto, par.Value, produto.Preco));
            }

            compra.Itens = itensValidos;
            return true;
        }
    }
}
