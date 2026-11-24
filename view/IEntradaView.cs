namespace ProjetoSOLID.view
{
    public interface IEntradaView
    {
        /// <summary>Pergunta um ID. Devolve null se a pessoa digitar algo inválido.</summary>
        int? LerId(string mensagem);
    }
}
