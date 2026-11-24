using Microsoft.Data.Sqlite;

namespace ProjetoSOLID.dados
{
    /// <summary>Cria as tabelas na primeira execução.</summary>
    public class DatabaseInitializer
    {
        private readonly IConnectionFactory factory;

        public DatabaseInitializer(IConnectionFactory factory)
        {
            this.factory = factory;
        }

        public void Inicializar()
        {
            using (SqliteConnection conexao = factory.Criar())
            using (SqliteCommand cmd = conexao.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Produto (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome      TEXT NOT NULL,
                        Preco     TEXT NOT NULL,
                        Descricao TEXT
                    );

                    CREATE TABLE IF NOT EXISTS Compra (
                        Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                        Cliente    TEXT NOT NULL,
                        DataCompra TEXT NOT NULL
                    );

                    -- Tabela de ligação da relação n:n entre Compra e Produto
                    CREATE TABLE IF NOT EXISTS CompraProduto (
                        CompraId      INTEGER NOT NULL,
                        ProdutoId     INTEGER NOT NULL,
                        Quantidade    INTEGER NOT NULL,
                        PrecoUnitario TEXT NOT NULL,
                        PRIMARY KEY (CompraId, ProdutoId),
                        FOREIGN KEY (CompraId)  REFERENCES Compra(Id)  ON DELETE CASCADE,
                        FOREIGN KEY (ProdutoId) REFERENCES Produto(Id) ON DELETE RESTRICT
                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
