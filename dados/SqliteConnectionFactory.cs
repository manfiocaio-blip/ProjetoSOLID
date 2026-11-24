using Microsoft.Data.Sqlite;

namespace ProjetoSOLID.dados
{
    public class SqliteConnectionFactory : IConnectionFactory
    {
        private readonly string connectionString;

        public SqliteConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqliteConnection Criar()
        {
            SqliteConnection conexao = new SqliteConnection(connectionString);
            conexao.Open();

            // O SQLite deixa as chaves estrangeiras desligadas por padrão; ligamos aqui.
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();
            }
            return conexao;
        }
    }
}
