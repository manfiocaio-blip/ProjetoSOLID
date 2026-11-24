namespace ProjetoSOLID.view
{
    public interface IMenuView
    {
        /// <summary>Mostra um menu e devolve o número escolhido (0 = sair/voltar, -1 = inválido).</summary>
        int ExibirMenu(string titulo, string[] opcoes, string textoZero);
        void AguardarTecla();
    }
}
