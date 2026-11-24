# ProjetoSOLID – Loja de Carros (CRUD de Carros e Compras)

Aplicação de console em C# (.NET) com **CRUD de Carros (Produto)** e **CRUD de Compras**,
seguindo **MVC** e os princípios **SOLID**, com banco de dados **SQLite**.

## Relação n:n
Uma compra pode ter vários carros e um carro pode estar em várias compras.
Isso é feito pela tabela de ligação `CompraProduto` (CompraId, ProdutoId, Quantidade, PrecoUnitario).

## Arquitetura (MVC)
- `model/` – Produto, Compra, ItemCompra
- `view/` – telas de console (menus, cadastro, exibição)
- `controller/` – ProdutoController e CompraController
- `dados/` – IRepository<T> e implementações com SQLite

## SOLID
- **S**: cada classe tem uma responsabilidade (view só mostra/lê, controller coordena, repository acessa o banco).
- **O**: novos repositórios/views podem ser criados sem alterar os controllers.
- **L**: qualquer IRepository<T> pode substituir outro (ex.: lista em memória x SQLite).
- **I**: interfaces pequenas (ICadastroView, IAtualizacaoView, IEntradaView, IMenuView...).
- **D**: controllers dependem de interfaces; as classes concretas são montadas só no `Program.cs`.

## Como rodar
```
dotnet run
```
O arquivo `loja_carros.db` é criado automaticamente na primeira execução.
