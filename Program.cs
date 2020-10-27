using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace AlgoritmoBusca
{
    // Para a comparação dos nomes foram usadas dois metodos da classe String:
    //
    // Como cada posição do vetor foi lida como uma linha inteira do registro usou-se
    // a função Split() que divide a string de acordo com um separador,
    // e retorna um vetor de string, para comparação pegou-se a primeira posição (first_name)
    //
    // A função usada para comparação foi a String.Compare(), ela recebe duas strings
    // e retorna um número relativo a posição entre elas:
    // 0 é igual, -1 a primeira é menor, 1 a primeira é maior

    class Program
    {
        // variável usada para contar o número de comparações
        static int comparacoes = 0;
        // variável usada para ler a primeira linha do registro (cabeçalho)
        static string firstLine;

        static void Main(string[] args)
        {
            // guarda o tamanho do registro
            int tamanho = 0;

            // guarda o caminho para o arquivo de registro, (.txt) nomes e se o usuario deseja fazer outra busca
            string dataPath, nameSearchPath, repeat;

            // vetor que irá copiar os dados do registro
            string[] data = new string[5000];
            
            // file que irá ler os nomes para a busca e guarda-los na lista "nomes"
            StreamReader nomesTemp;
            List <string> nomes = new List <string>();

            // executa enquanto os caminhos para os arquivos não existirem
            while (true)
            {                
                Console.WriteLine("Entre com o caminho para o arquivo que contenha os registros:");
                dataPath = Console.ReadLine();

                Console.WriteLine("\nEntre com o caminho para o arquivo que contenha os nomes a serem buscados: (.txt)");
                nameSearchPath = Console.ReadLine();

                if (!(File.Exists(dataPath) && File.Exists(nameSearchPath)))
                {
                    Console.WriteLine("\nUm dos caminhos informados não existe, verifique e tente novamente.\n");
                    Console.WriteLine();
                }
                else
                {
                    break;
                }
            };
            
            // prepara o arquivo com os nomes para leitura
            nomesTemp = new StreamReader(nameSearchPath);

            // lê cada nome e adiciona na lista correspondente
            while (!nomesTemp.EndOfStream)
                nomes.Add(nomesTemp.ReadLine());

            // fecha o arquivo de texto
            nomesTemp.Close();

            // executa enquanto o usuário quiser mudar o registro para comparações
            while (true)
            {
                // classe usada para ler o arquivo de registros
                TextFieldParser dataBase = new TextFieldParser(dataPath);
                firstLine = dataBase.ReadLine();

                // lê cada linha do arquivo de registro e coloca na posição correspondente do vetor
                // só termina a execução quando ler todo o arquivo
                while (!dataBase.EndOfData)
                {
                    data[tamanho] = dataBase.ReadLine();
                    tamanho++;
                };

                // Coloca na tela o número de registros do arquivo
                Console.WriteLine("\nNúmero de registros: " + tamanho);
                Console.WriteLine("--------------------\n\n");

                // executa a busca para cada nome do arquivo de texto
                // zera o numero de comparações antes de ir para a próxima busca
                foreach (string nome in nomes)
                {
                    Console.WriteLine("--");

                    Console.WriteLine("Busca Binária: " + nome + "\n");
                    BuscaBinaria(data, nome, 0, tamanho - 1);
                    Console.WriteLine("\nNumero de comparações (Busca Binária): " + comparacoes + "\n\n");
                    
                    comparacoes = 0;

                    Console.WriteLine("Busca Sequencial: " + nome + "\n");
                    BuscaSequencial(data, nome, tamanho);
                    Console.WriteLine("\nNumero de comparações (Busca Sequencial): " + comparacoes + "\n\n");
                    
                    comparacoes = 0;                                                    
                };
                
                // fecha o arquivo dos registros, reseta o vetor que copiou os registros e zera o tamanho
                // para uma nova busca, se for necessário
                dataBase.Close();
                Array.Clear(data, 0, data.Length);
                tamanho = 0;

                Console.WriteLine("Deseja realizar uma busca com um arquivo de registros diferentes? (S/N)");
                repeat = Console.ReadLine();

                if (String.Compare(repeat, "S", true) == 0)
                {
                    Console.Clear();

                    Console.WriteLine("Entre com o caminho para o arquivo que contenha os registros:");
                    dataPath = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }
        }

        static void BuscaSequencial(string[] v, string buscado, int tamanho)
        {
            // Percorre todo o vetor
            for (int i = 0; i < tamanho; i++)
            {
                comparacoes++;
                // Compara na posição i se o valor buscado é igual
                if (String.Compare(v[i].Split(',')[0], buscado) == 0)
                {
                    // Se o nome for encontrado, escreve a linha e termina a busca
                    Console.WriteLine(firstLine + "\n" + v[i]);
                    return;
                }
            }

            // Caso o nome não seja encontrado, escreve que o nome não foi encontrado
            Console.WriteLine("Item não existente/encontrado no conjunto de dados\n");
        }
        static void BuscaBinaria(string[] v, string buscado, int posInicial, int posFinal)
        {
            // Descobre a posição do meio
            int meio = (posFinal + posInicial) / 2;

            // Só compara os valores se a posição final for menor que a posição inicial no subvetor 
            if (posFinal >= posInicial)
            {
                comparacoes++;
                if (String.Compare(buscado, v[meio].Split(',')[0]) == -1)
                {
                    // valor buscado é menor
                    BuscaBinaria(v, buscado, posInicial, meio - 1);
                }
                else if (String.Compare(buscado, v[meio].Split(',')[0]) == 1)
                {
                    // valor buscado é maior
                    BuscaBinaria(v, buscado, meio + 1, posFinal);
                }
                else
                {
                    // valor buscado é igual
                    // escreve a linha e termina a busca
                    Console.WriteLine(firstLine + "\n" + v[meio]);
                    return;
                }
            }
            else
            {
                // se a posição final é menor que a inicial, e a chave não foi encontrada
                // escreve que o nome não foi encontrado
                Console.WriteLine("Item não existente/encontrado no conjunto de dados\n");
                return;
            }
        }
    }
}