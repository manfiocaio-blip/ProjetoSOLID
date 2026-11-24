using Microsoft.Data.Sqlite;
using ProjetoSOLID.model;
using System;
using System.Collections.Generic;

namespace ProjetoSOLID.dados
{
    public class ProdutoRepository : IRepository<Produto>
    {
        private readonly IConnectionFactory factory;

        public ProdutoRepository(IConnectionFactory factory)
        {
            this.factory = factory;
        }

        public void Adicionar(Produto novo)
        {
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText =
                    "INSERT INTO Produto (Nome, Preco, Descricao) VALUES ($nome, $preco, $desc); " +
                    "SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("$nome", novo.Nome);
                cmd.Parameters.AddWithValue("$preco", novo.Preco);
                cmd.Parameters.AddWithValue("$desc", novo.Descricao ?? "");

                novo.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<Produto> ListarTodos()
        {
            List<Produto> lista = new List<Produto>();

            using (SqliteConnection conexao = factory.Criar())
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Nome, Preco, Descricao FROM Produto ORDER BY Id";
                using (SqliteDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                        lista.Add(Ler(r));
                }
            }
            return lista;
        }

        public Produto BuscarPorId(int id)
        {
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText = "SELECT Id, Nome, Preco, Descricao FROM Produto WHERE Id = $id";
                cmd.Parameters.AddWithValue("$id", id);
                using (SqliteDataReader r = cmd.ExecuteReader())
                {
                    return r.Read() ? Ler(r) : null;
                }
            }
        }

        public void Atualizar(Produto entidade)
        {
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText =
                    "UPDATE Produto SET Nome = $nome, Preco = $preco, Descricao = $desc WHERE Id = $id";
                cmd.Parameters.AddWithValue("$nome", entidade.Nome);
                cmd.Parameters.AddWithValue("$preco", entidade.Preco);
                cmd.Parameters.AddWithValue("$desc", entidade.Descricao ?? "");
                cmd.Parameters.AddWithValue("$id", entidade.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Remover(Produto entidade)
        {
            Remover(entidade.Id);
        }

        public void Remover(int id)
        {
            try
            {
                using (SqliteConnection conexao = factory.Criar())
                using (SqliteCommand cmd = conexao.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Produto WHERE Id = $id";
                    cmd.Parameters.AddWithValue("$id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                // 19 = violação de restrição (o carro já está em alguma compra)
                throw new InvalidOperationException(
                    "Este carro já faz parte de uma compra e não pode ser excluído.");
            }
        }

        private static Produto Ler(SqliteDataReader r)
        {
            return new Produto(
                r.GetInt32(0),
                r.GetString(1),
                r.GetDecimal(2),
                r.IsDBNull(3) ? "" : r.GetString(3));
        }
    }
}
