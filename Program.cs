using ProjetoSOLID.controller;
using ProjetoSOLID.dados;
using ProjetoSOLID.model;
using ProjetoSOLID.view;
using System;
using System.Text;

namespace ProjetoSOLID
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // ---- Banco de dados (SQLite: cria o arquivo loja_carros.db sozinho) ----
            IConnectionFactory factory = new SqliteConnectionFactory("Data Source=loja_carros.db");
            new DatabaseInitializer(factory).Inicializar();

            // ---- Repositórios ----
            IRepository<Produto> produtoRepository = new ProdutoRepository(factory);
            IRepository<Compra> compraRepository = new CompraRepository(factory);

            // ---- Views ----
            IProdutoView produtoView = new ProdutoView();
            CadastroProduto cadastroProduto = new CadastroProduto();
            ICompraView compraView = new CompraView();
            CadastroCompra cadastroCompra = new CadastroCompra();
            IMenuView menuView = new MenuConsoleView();
            IEntradaView entradaView = new EntradaConsoleView();

            // ---- Controllers ----
            ProdutoController produtoController = new ProdutoController(
                produtoView, produtoRepository, cadastroProduto, cadastroProduto);

            CompraController compraController = new CompraController(
                compraView, compraRepository, produtoRepository,
                produtoView, cadastroCompra, cadastroCompra);

            // ---- Menu principal ----
            int opcao;
            do
            {
                opcao = menuView.ExibirMenu(
                    "LOJA DE CARROS",
                    new[] { "Gerenciar carros", "Gerenciar compras" },
                    "Sair");

                switch (opcao)
                {
                    case 1:
                        MenuCarros(menuView, entradaView, produtoController);
                        break;
                    case 2:
                        MenuCompras(menuView, entradaView, compraController);
                        break;
                    case 0:
                        Console.Clear();
                        Console.WriteLine("Encerrando o sistema...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        menuView.AguardarTecla();
                        break;
                }
            } while (opcao != 0);
        }

        static void MenuCarros(IMenuView menu, IEntradaView entrada, ProdutoController controller)
        {
            string[] opcoes =
            {
                "Cadastrar carro", "Listar carros", "Buscar carro",
                "Atualizar carro", "Excluir carro"
            };

            int opcao;
            do
            {
                opcao = menu.ExibirMenu("CARROS", opcoes, "Voltar");
                if (opcao == 0) break;

                Console.Clear();
                int? id;

                switch (opcao)
                {
                    case 1:
                        controller.Cadastrar();
                        break;
                    case 2:
                        controller.Listar();
                        break;
                    case 3:
                        id = entrada.LerId("Informe o ID do carro: ");
                        if (id.HasValue) controller.Exibir(id.Value);
                        break;
                    case 4:
                        id = entrada.LerId("Informe o ID do carro: ");
                        if (id.HasValue) controller.Atualizar(id.Value);
                        break;
                    case 5:
                        id = entrada.LerId("Informe o ID do carro: ");
                        if (id.HasValue) controller.Remover(id.Value);
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                menu.AguardarTecla();
            } while (true);
        }

        static void MenuCompras(IMenuView menu, IEntradaView entrada, CompraController controller)
        {
            string[] opcoes =
            {
                "Registrar compra", "Listar compras", "Buscar compra",
                "Atualizar compra", "Excluir compra"
            };

            int opcao;
            do
            {
                opcao = menu.ExibirMenu("COMPRAS", opcoes, "Voltar");
                if (opcao == 0) break;

                Console.Clear();
                int? id;

                switch (opcao)
                {
                    case 1:
                        controller.Cadastrar();
                        break;
                    case 2:
                        controller.Listar();
                        break;
                    case 3:
                        id = entrada.LerId("Informe o ID da compra: ");
                        if (id.HasValue) controller.Exibir(id.Value);
                        break;
                    case 4:
                        id = entrada.LerId("Informe o ID da compra: ");
                        if (id.HasValue) controller.Atualizar(id.Value);
                        break;
                    case 5:
                        id = entrada.LerId("Informe o ID da compra: ");
                        if (id.HasValue) controller.Remover(id.Value);
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                menu.AguardarTecla();
            } while (true);
        }
    }
}
