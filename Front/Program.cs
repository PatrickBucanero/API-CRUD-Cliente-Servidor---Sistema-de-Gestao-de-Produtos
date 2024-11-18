using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Front
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string jsonResponse = "{ \"key\": \"value\" }"; // Exemplo de JSON. Substitua isso pelo conteúdo do arquivo ou resposta da API.

            try
            {
                var parsedJson = JObject.Parse(jsonResponse);
                Console.WriteLine("O JSON é válido.");
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"O JSON é inválido: {ex.Message}");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Front());
        }
    }
}
