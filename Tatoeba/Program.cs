using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tatoeba
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    
        public static void getLangueageCodes() {
            StreamReader SR = new StreamReader(Path.GetFullPath("Files/sentences.csv"));

            List<string> langList = new List<string>();
            string line = SR.ReadLine();

            while (line != null)
            {
                string code = line.Split('\t')[1];
                if (!langList.Contains(code)) langList.Add(code);
                line = SR.ReadLine();
            }
            SR.Close();

            StreamWriter SW = new StreamWriter(Path.GetFullPath("Files/languageCodeList.csv"));
            foreach (string code in langList) {
                SW.WriteLine(code + ",");
            }

            SW.Close();
        }

        public static void GetStats()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            StreamReader SR = new StreamReader(Path.GetFullPath("Files/SentenceLists/jbo.csv"));
            string line = SR.ReadLine();
            while (line != null) {
                string[] words = line.Split('\t')[2].Split(' ');
                foreach (string word in words) {
                    int currentCount;
                    if (dict.ContainsKey(word)) {
                        dict.TryGetValue(word, out currentCount);
                        dict[word] = currentCount + 1;
                    }
                    else {
                        dict.Add(word, 1);
                    }
                }
                line = SR.ReadLine();
            }

            SR.Close();

            var items = from pair in dict orderby pair.Value descending select pair;

            StreamWriter SW = new StreamWriter(Path.GetFullPath("Files/LanguageStats/jbo.csv"));
            foreach (KeyValuePair<string, int> pair in items) {
                SW.WriteLine($"{pair.Key},{pair.Value}");
            }
            SW.Close();
        }

        public static void refactorSentenceList() {
            List<string> keys = new List<string>();
            StreamReader statsSR = new StreamReader(Path.GetFullPath("Files/LanguageStats/jbo.csv"));
            string line = statsSR.ReadLine();
            for (int i = 0; i < 250; i++)
            {
                keys.Add(line.Split(',')[0]);
            }
            keys.Add("00000");
            statsSR.Close();

            StreamReader sentencesSR = new StreamReader(Path.GetFullPath("Files/SentenceLists/jbo.csv"));
            string line2 = sentencesSR.ReadLine();
            string[] arr = line2.Split(',');

            StreamWriter SW = new StreamWriter(Path.GetFullPath("Files/SentenceLists/jbo2.csv"));

            while (line2 != null) {
                foreach (string item in keys) {
                    foreach (string word in arr) {
                        if (word == item) {
                            SW.WriteLine($"{item},{arr[2]}");
                        }
                    }
                }
            }

            sentencesSR.Close();
        }
    
    }
}
