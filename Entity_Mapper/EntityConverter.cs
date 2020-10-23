using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Entity_Mapper
{
    public class EntityConverter
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

        private List<string> EntityNames = new List<string>();

        private List<string> ViewObjectNames = new List<string>();

        public EntityConverter(string path)
        {
            var pathItens = path.Split("\\");

            FileName = pathItens[pathItens.Length - 1];

            for(int i = 0; i <= pathItens.Length - 2; i++)
            {
                FilePath += pathItens[i] + "\\";
            }
        }

        public void FileReader()
        {
            using (FileStream fs = new FileStream(FilePath + FileName,
                FileMode.OpenOrCreate))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(" ");
                    
                    if(line[0].Contains(");") || line[0].Contains("CREATE") || line[0].Contains("CONSTRAINT"))
                    {
                        continue;
                    }

                    if (line[0].Contains("\""))
                    {
                        line[0] = line[0].Replace("\"", "");
                        line[0] = line[0].Replace("\t", "");
                        EntityNames.Add(line[0]);
                    }
                    else
                    {
                        line[0] = line[0].Replace("\t", "");
                        EntityNames.Add(line[0]);
                    }
                }
            }
        }

        public void MapViewObject()
        {
            foreach (string name in EntityNames)
            {
                if(name.Contains("_"))
                {
                    TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
                    string nameCamelCase = txtInfo.ToTitleCase(name.ToLower()).Replace("_", string.Empty);
                    nameCamelCase = nameCamelCase.First().ToString().ToLowerInvariant() + nameCamelCase.Substring(1);

                    ViewObjectNames.Add(nameCamelCase);
                }
                else
                {
                    ViewObjectNames.Add(name.ToLower());
                }
            }
        }

        public void WriteFile()
        {
            using (FileStream fs = new FileStream(FilePath + Guid.NewGuid().ToString() + FileName,
                FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach(string item in EntityNames)
                {
                    sw.WriteLine(item);
                }

                sw.Write("\n");

                foreach (string item in ViewObjectNames)
                {
                    sw.WriteLine(item);
                }

                sw.Write("\n");

                for (int i = 0; i <= EntityNames.Count - 1; i++)
                {
                    sw.WriteLine($"{EntityNames[i]} = origin.{ViewObjectNames[i]},");
                }

                sw.Write("\n");

                for (int i = 0; i <= ViewObjectNames.Count - 1; i++)
                {
                    sw.WriteLine($"{ViewObjectNames[i]} = origin.{EntityNames[i]},");
                }
            }
        }
    }
}
