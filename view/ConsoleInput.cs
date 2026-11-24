using System;
using System.Globalization;

namespace ProjetoSOLID.view
{
    /// <summary>Funções de leitura do teclado que repetem a pergunta até a pessoa digitar algo válido.</summary>
    public static class ConsoleInput
    {
        public static string LerTexto(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                string texto = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(texto))
                    return texto.Trim();

                Console.WriteLine("Este campo não pode ficar vazio.");
            }
        }

        // Se a pessoa só apertar Enter, mantém o valor atual (usado nas atualizações).
        public static string LerTextoOpcional(string mensagem, string atual)
        {
            Console.Write($"{mensagem} [{atual}]: ");
            string texto = Console.ReadLine();
            return string.IsNullOrWhiteSpace(texto) ? atual : texto.Trim();
        }

        public static int LerInt(string mensagem, int minimo)
        {
            while (true)
            {
                Console.Write(mensagem);
                if (int.TryParse(Console.ReadLine(), out int valor) && valor >= minimo)
                    return valor;

                Console.WriteLine($"Digite um número inteiro maior ou igual a {minimo}.");
            }
        }

        public static decimal LerDecimal(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);
                if (TentarDecimal(Console.ReadLine(), out decimal valor) && valor >= 0)
                    return valor;

                Console.WriteLine("Digite um valor válido, por exemplo 45000,50.");
            }
        }

        public static decimal LerDecimalOpcional(string mensagem, decimal atual)
        {
            while (true)
            {
                Console.Write($"{mensagem} [{atual}]: ");
                string texto = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(texto))
                    return atual;

                if (TentarDecimal(texto, out decimal valor) && valor >= 0)
                    return valor;

                Console.WriteLine("Digite um valor válido, por exemplo 45000,50.");
            }
        }

        public static bool LerSimNao(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem + " (s/n): ");
                string r = (Console.ReadLine() ?? "").Trim().ToLower();
                if (r == "s") return true;
                if (r == "n") return false;
            }
        }

        private static bool TentarDecimal(string texto, out decimal valor)
        {
            return decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture, out valor)
                || decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
        }
    }
}
