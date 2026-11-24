using ProjetoSOLID.dados;
using ProjetoSOLID.model;
using ProjetoSOLID.view;
using System;
using System.Collections.Generic;

namespace ProjetoSOLID.controller
{
    public class ProdutoController
    {
        private readonly IRepository<Produto> repository;
        private readonly IProdutoView view;
        private readonly ICadastroView<Produto> cadastroView;
        private readonly IAtualizacaoView<Produto> atualizacaoView;

        public ProdutoController(
            IProdutoView view,
            IRepository<Produto> repository,
            ICadastroView<Produto> cadastroView,
            IAtualizacaoView<Produto> atualizacaoView)
        {
            this.view = view;
            this.repository = repository;
            this.cadastroView = cadastroView;
            this.atualizacaoView = atualizacaoView;
        }

        public void Cadastrar()
        {
            Produto produto = cadastroView.ObterDados();

            repository.Adicionar(produto);

            cadastroView.ExibirMensagem(
                $"Carro cadastrado com sucesso! (ID {produto.Id})"
            );
        }

        public void Listar()
        {
            List<Produto> produtos = repository.ListarTodos();

            if (produtos.Count == 0)
            {
                cadastroView.ExibirMensagem("Nenhum carro cadastrado.");
                return;
            }

            foreach (Produto produto in produtos)
                view.MostrarProduto(produto);
        }

        public Produto Buscar(int id)
        {
            return repository.BuscarPorId(id);
        }

        public void Exibir(int id)
        {
            Produto produto = Buscar(id);

            if (produto == null)
                cadastroView.ExibirMensagem("Carro não encontrado.");
            else
                view.MostrarProduto(produto);
        }

        public void Atualizar(int id)
        {
            Produto existente = Buscar(id);

            if (existente == null)
            {
                cadastroView.ExibirMensagem("Carro não encontrado.");
                return;
            }

            Produto atualizado = atualizacaoView.ObterDadosAtualizacao(existente);
            repository.Atualizar(atualizado);

            cadastroView.ExibirMensagem("Carro atualizado com sucesso!");
        }

        public void Remover(int id)
        {
            if (Buscar(id) == null)
            {
                cadastroView.ExibirMensagem("Carro não encontrado.");
                return;
            }

            try
            {
                repository.Remover(id);
                cadastroView.ExibirMensagem("Carro excluído com sucesso!");
            }
            catch (InvalidOperationException ex)
            {
                cadastroView.ExibirMensagem(ex.Message);
            }
        }
    }
}
