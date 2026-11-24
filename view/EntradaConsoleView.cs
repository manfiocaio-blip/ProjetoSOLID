using System;

namespace ProjetoSOLID.view
{
    public class EntradaConsoleView : IEntradaView
    {
        public int? LerId(string mensagem)
        {
            Console.Write(mensagem);

            if (int.TryParse(Console.ReadLine(), out int id))
                return id;

            Console.WriteLine("ID inválido.");
            return null;
        }
    }
}
