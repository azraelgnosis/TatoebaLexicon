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
        //public List<string> VM.langSentences { get; set; }
        public List<string> sentenceList { get; set; }
        public ViewModel VM;

        public SentenceViewComponent(IViewModel viewModel) {
            VM = (ViewModel)viewModel;
            if (VM.langSentences == null || VM.langSentences[0].Split("\t")[1] != VM.currentLang.ToString()) { GetAllSentences(); }
            //GetSentences();
            GetMySentences();
            if (!String.IsNullOrEmpty(VM.filter)) filterSentences();
        }
        
        public IViewComponentResult Invoke() {
            return View("SentenceList", sentenceList);
        }

        private void GetAllSentences() {
            VM.langSentences = new List<string>();
            StreamReader SR = new StreamReader(Path.GetFullPath($"Files/SentenceLists/{VM.currentLang.ToString()}.csv"));
            string line = SR.ReadLine();
            while (line != null) {
                VM.langSentences.Add(line);
                line = SR.ReadLine();
            }
            SR.Close();
        }

        private void GetMySentences() {
            sentenceList = new List<string>();
            foreach (string row in VM.langSentences) {
                Regex rgx = new Regex("[^A-z0-9' ]");
                string sentence = row.Split("\t")[2];
                string phrase = rgx.Replace(sentence, "");
                string[] words = phrase.Split(' ');
                bool exited = false;

                foreach (string word in words) {
                    if (!VM.LemmaList.Exists(l => l.lexeme.ToLower() == word.ToLower() && l.lang.ToString() == VM.currentLang.ToString()))
                    {
                        exited = true; break;
                    }
                }
                if (!exited) {
                    sentenceList.Add(sentence);
                }
            }
        }

        private void GetSentences() {
            sentenceList = new List<string>();
            StreamReader SR = new StreamReader(Path.GetFullPath($"Files/SentenceLists/{VM.currentLang.ToString()}.csv"));
            string line = SR.ReadLine();

            while (line != null) {
                string[] arr = line.Split('\t');
                string language = arr[1];

                string phrase = arr[2];
                Regex rgx = new Regex("[^A-z0-9' ]");
                string phraseNoPunc = rgx.Replace(arr[2], "");

                string[] words = phraseNoPunc.Split(" ");
                bool exited = false;
                foreach (string word in words) {
                    if (!VM.LemmaList.Exists(l => l.lexeme.ToLower() == word.ToLower() && l.lang.ToString() == language)) {
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

        private void filterSentences() {
            sentenceList = sentenceList.FindAll(s => s.Contains(VM.filter));
        }
    }
}
