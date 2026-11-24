using System;

namespace ProjetoSOLID.view
{
    public class MenuConsoleView : IMenuView
    {
        public int ExibirMenu(string titulo, string[] opcoes, string textoZero)
        {
            Console.Clear();

            Console.WriteLine($"=== {titulo} ===");
            Console.WriteLine();

            for (int i = 0; i < opcoes.Length; i++)
                Console.WriteLine($"{i + 1} - {opcoes[i]}");

            Console.WriteLine($"0 - {textoZero}");
            Console.WriteLine();
            Console.Write("Escolha uma opção: ");

            if (int.TryParse(Console.ReadLine(), out int opcao))
                return opcao;

            return -1;
        }

        public void AguardarTecla()
        {
            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
