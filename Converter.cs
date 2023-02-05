using Newtonsoft.Json;
using System.Xml.Serialization;

namespace flare
{
    class Converter
    {
        public void Print()
        {
            string format = "txt";
            Console.WriteLine("Введите путь к файлу, который вы хотите открыть (вместе с названием!)");
            string filepath = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("F1 - сохранить файла | Esc - выйти");
            Console.WriteLine("------------------------------------------");
            if (filepath.Contains(".txt"))
            {
                DeserializeTxt(filepath);
                format = "txt";
            }
            else if (filepath.Contains(".json"))
            {
                DeserializeJson(filepath);
                format = "json";
            }
            else if (filepath.Contains(".xml"))
            {
                DeserializeXml(filepath);
                format = "xml";
            }
            ConsoleKeyInfo pip = Console.ReadKey();
            switch (pip.Key)
            {
                case ConsoleKey.F1:
                    Serialize(filepath, format);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
        private void Serialize(string Path, string format)
        {
            List<Parameters> Params = new List<Parameters>();
            string text = File.ReadAllText(Path);
            string[] lines = File.ReadAllLines(Path);
            int line = 0;
            if (format == "txt")
            {
                for (int i = 0; i < lines.Length / 3; i++)
                {
                    string length = File.ReadLines(Path).Skip(0 + line).First();
                    string width = File.ReadLines(Path).Skip(1 + line).First();
                    string height = File.ReadLines(Path).Skip(2 + line).First();
                    Parameters pyp = new Parameters(length, width, height);
                    Params.Add(pyp);
                    line += 3;
                }
            }
            else if (format == "json")
            {
                Params = JsonConvert.DeserializeObject<List<Parameters>>(text);
            }
            else if (format == "xml")
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Parameters>));
                using (FileStream fs = new FileStream(Path, FileMode.Open))
                {
                    Params = (List<Parameters>)xml.Deserialize(fs);
                }
            }

            Console.Clear();
            Console.WriteLine("Введите путь для сохранения файла");
            string savefilepath = Console.ReadLine();
            if (savefilepath.Contains(".json"))
            {
                string json = JsonConvert.SerializeObject(Params);
                File.WriteAllText(savefilepath, json);
            }
            else if (savefilepath.Contains(".xml"))
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Parameters>));
                using (FileStream fs = new FileStream(savefilepath, FileMode.OpenOrCreate))
                {
                    xml.Serialize(fs, Params);
                }
            }
            else if (savefilepath.Contains(".txt"))
            {
                foreach (Parameters param in Params)
                {
                    File.AppendAllText(savefilepath, param.length + "\n");
                    File.AppendAllText(savefilepath, param.height + "\n");
                    File.AppendAllText(savefilepath, param.width + "\n");
                }
            }
        }
        static void DeserializeTxt(string path)
        {
            string text = File.ReadAllText(path);
            Console.WriteLine(text);
        }
        static void DeserializeJson(string path)
        {
            string text = File.ReadAllText(path);
            List<Parameters> Params = new List<Parameters>();
            List<Parameters> convert = JsonConvert.DeserializeObject<List<Parameters>>(text);
            foreach (Parameters i in convert)
            {
                Console.WriteLine(i.length);
                Console.WriteLine(i.width);
                Console.WriteLine(i.height);
                Params.Add(i);
            }
        }
        static void DeserializeXml(string path)
        {
            List<Parameters> i;
            List<Parameters> Prm = new List<Parameters>();
            XmlSerializer xml = new XmlSerializer(typeof(List<Parameters>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                i = (List<Parameters>)xml.Deserialize(fs);
            }
            foreach (var n in i)
            {
                Console.WriteLine(n.length);
                Console.WriteLine(n.width);
                Console.WriteLine(n.height);
                Prm.Add(n);
            }
        }
        public class Parameters
        {
            public Parameters(string length, string width, string height)
            {
                this.length = length;
                this.width = width;
                this.height = height;
            }
            public string length;
            public string width;
            public string height;
        }

    }
}
