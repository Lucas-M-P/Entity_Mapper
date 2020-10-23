using System;
using System.IO;

namespace Entity_Mapper
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath;

            Console.WriteLine("Digite o caminho completo do arquivo: ");
            filePath = Console.ReadLine();

            EntityConverter entityConverter = new EntityConverter(filePath);

            Console.WriteLine("Mapeando a entidade");
            entityConverter.FileReader();

            Console.WriteLine("Mapeando a View Object");
            entityConverter.MapViewObject();

            Console.WriteLine("Criando o arquivo e mapeando a converter");
            entityConverter.WriteFile();

            Console.WriteLine("Tabela convertida");
        }
    }
}
