using Microsoft.Data.Sqlite;

namespace ProjetoSOLID.dados
{
    /// <summary>Quem precisa de conexão pede a esta interface (Inversão de Dependência).</summary>
    public interface IConnectionFactory
    {
        SqliteConnection Criar();
    }
}
