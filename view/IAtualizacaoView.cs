namespace ProjetoSOLID.view
{
    /// <summary>
    /// Interface separada de ICadastroView (Segregação de Interfaces):
    /// quem só cadastra não precisa conhecer a atualização.
    /// </summary>
    public interface IAtualizacaoView<T>
    {
        T ObterDadosAtualizacao(T atual);
    }
}
