using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Tatoeba.Models;

namespace Tatoeba.ViewComponents
{
    public class SentenceViewComponent : ViewComponent
    {
        public List<string> sentenceList { get; set; }
        public ViewModel VM;

        public SentenceViewComponent(IViewModel viewModel) {
            VM = (ViewModel)viewModel;
            GetSentences(VM.LemmaList);
        }

        public IViewComponentResult Invoke() {
            return View("SentenceList", sentenceList);
        }

        private void GetSentences(List<Lemma> lemmaList) {
            sentenceList = new List<string>();
            StreamReader SR = new StreamReader(Path.GetFullPath($"Files/SentenceLists/{VM.currentLang.ToString()}.csv"));
            string line = SR.ReadLine();

            while (line != null) {
                string[] arr = line.Split('\t');
                string language = arr[1];

                string phrase = arr[2];
                Regex rgx = new Regex("[^A-z0-9 ]");
                string phraseNoPunc = rgx.Replace(arr[2], "");

                string[] words = phraseNoPunc.Split(" ");
                bool exited = false;
                foreach (string word in words) {
                    if (!lemmaList.Exists(l => l.lexeme.ToLower() == word.ToLower() && l.lang.ToString() == language)) {
                        exited = true;
                        break;
                    }
                }
                if (!exited) {
                    sentenceList.Add(phrase);
                }
                line = SR.ReadLine();
            }
            SR.Close();
        }
    }
}
