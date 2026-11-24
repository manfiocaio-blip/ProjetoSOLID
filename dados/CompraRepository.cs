using Microsoft.Data.Sqlite;
using ProjetoSOLID.model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ProjetoSOLID.dados
{
    public class CompraRepository : IRepository<Compra>
    {
        private readonly IConnectionFactory factory;

        public CompraRepository(IConnectionFactory factory)
        {
            this.factory = factory;
        }

        public void Adicionar(Compra novo)
        {
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteTransaction tx = conexao.BeginTransaction())
            {
                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText =
                        "INSERT INTO Compra (Cliente, DataCompra) VALUES ($cliente, $data); " +
                        "SELECT last_insert_rowid();";
                    cmd.Parameters.AddWithValue("$cliente", novo.Cliente);
                    cmd.Parameters.AddWithValue("$data", novo.Data.ToString("o"));
                    novo.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                InserirItens(conexao, tx, novo);
                tx.Commit();
            }
        }

        public List<Compra> ListarTodos()
        {
            List<Compra> lista = new List<Compra>();

            using (SqliteConnection conexao = factory.Criar())
            {
                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Cliente, DataCompra FROM Compra ORDER BY Id";
                    using (SqliteDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            lista.Add(LerCompra(r));
                    }
                }

                foreach (Compra compra in lista)
                    CarregarItens(conexao, compra);
            }
            return lista;
        }

        public Compra BuscarPorId(int id)
        {
            using (SqliteConnection conexao = factory.Criar())
            {
                Compra compra = null;

                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Cliente, DataCompra FROM Compra WHERE Id = $id";
                    cmd.Parameters.AddWithValue("$id", id);
                    using (SqliteDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                            compra = LerCompra(r);
                    }
                }

                if (compra != null)
                    CarregarItens(conexao, compra);

                return compra;
            }
        }

        public void Atualizar(Compra entidade)
        {
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteTransaction tx = conexao.BeginTransaction())
            {
                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = "UPDATE Compra SET Cliente = $cliente WHERE Id = $id";
                    cmd.Parameters.AddWithValue("$cliente", entidade.Cliente);
                    cmd.Parameters.AddWithValue("$id", entidade.Id);
                    cmd.ExecuteNonQuery();
                }

                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = "DELETE FROM CompraProduto WHERE CompraId = $id";
                    cmd.Parameters.AddWithValue("$id", entidade.Id);
                    cmd.ExecuteNonQuery();
                }

                InserirItens(conexao, tx, entidade);
                tx.Commit();
            }
        }

        public void Remover(Compra entidade)
        {
            Remover(entidade.Id);
        }

        public void Remover(int id)
        {
            // Os itens são apagados junto (ON DELETE CASCADE na tabela CompraProduto).
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Compra WHERE Id = $id";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // ---------- auxiliares ----------

        private static void InserirItens(SqliteConnection conexao, SqliteTransaction tx, Compra compra)
        {
            foreach (ItemCompra item in compra.Itens)
            {
                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText =
                        "INSERT INTO CompraProduto (CompraId, ProdutoId, Quantidade, PrecoUnitario) " +
                        "VALUES ($compra, $produto, $qtd, $preco)";
                    cmd.Parameters.AddWithValue("$compra", compra.Id);
                    cmd.Parameters.AddWithValue("$produto", item.Produto.Id);
                    cmd.Parameters.AddWithValue("$qtd", item.Quantidade);
                    cmd.Parameters.AddWithValue("$preco", item.PrecoUnitario);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void CarregarItens(SqliteConnection conexao, Compra compra)
        {
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT cp.Quantidade, cp.PrecoUnitario, p.Id, p.Nome, p.Preco, p.Descricao " +
                    "FROM CompraProduto cp " +
                    "JOIN Produto p ON p.Id = cp.ProdutoId " +
                    "WHERE cp.CompraId = $id";
                cmd.Parameters.AddWithValue("$id", compra.Id);

                using (SqliteDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Produto produto = new Produto(
                            r.GetInt32(2),
                            r.GetString(3),
                            r.GetDecimal(4),
                            r.IsDBNull(5) ? "" : r.GetString(5));

                        compra.Itens.Add(new ItemCompra(produto, r.GetInt32(0), r.GetDecimal(1)));
                    }
                }
            }
        }

        private static Compra LerCompra(SqliteDataReader r)
        {
            DateTime data = DateTime.Parse(r.GetString(2), null, DateTimeStyles.RoundtripKind);
            return new Compra(r.GetInt32(0), r.GetString(1), data);
        }
    }
}
