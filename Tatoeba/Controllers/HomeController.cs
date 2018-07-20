using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tatoeba.Models;

namespace Tatoeba.Controllers
{
    public class HomeController : Controller
    {
        ViewModel VM;
        string path;

        public HomeController(IViewModel viewModel) {
            VM = (ViewModel) viewModel;
            if (VM.user != null) path = Path.GetFullPath($"Files/Users/{VM.user}.csv");
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string name)
        {
            if (name != null)
            {
                VM.user = name;
                }
            if (VM.user != null) path = Path.GetFullPath($"Files/Users/{VM.user}.csv");
            GetLemmas();
            return View(VM);
        }

        [HttpPost]
        public IActionResult Index(ViewModel model) {
            VM.currentLang = model.currentLang;
            if (VM.currentLang != Language.Language) { GenSentenceList(VM.currentLang); }
            return View(VM);
        }

/*----------------------------------------------------------------------------*/

        [HttpPost]
        public IActionResult Add(ViewModel addModel)
        {
            AddLemma(addModel.lemma);
            return RedirectToAction("Index");
        }

/*----------------------------------------------------------------------------*/

        [HttpPost]
        public IActionResult Edit(Lemma editLemma) {
            EditLemma(editLemma);
            return RedirectToAction("Index");
        }

/*----------------------------------------------------------------------------*/

        [HttpPost]
        public IActionResult Delete(int deleteID)
        {
            DeleteLemma(deleteID);
            return RedirectToAction("Index");
        }

/*----------------------------------------------------------------------------*/
/*----------------------------------------------------------------------------*/

        private void GetLemmas()
        {
            VM.LemmaList = new List<Lemma>();
            VM.lemmaIDs = new List<int>();
            StreamReader SR = new StreamReader(path);
            string line = SR.ReadLine();
            while (line != null) {
                VM.LemmaList.Add(Lemma.FromLine(line));
                VM.lemmaIDs.Add(int.Parse(line.Split(',')[0]));
                line = SR.ReadLine();
            }
            SR.Close();
            VM.LemmaList.Sort();
        }

/*----------------------------------------------------------------------------*/

        private void AddLemma(Lemma lemma)
        {
            lemma.id = findMinID();
            StreamWriter SW = new StreamWriter(Path.GetFullPath(path), true);
            SW.WriteLine(lemma.ToLine());
            SW.Close();
        }

/*----------------------------------------------------------------------------*/

        private void EditLemma(Lemma editLemma) {
            int index = VM.LemmaList.FindIndex(l => l.id == editLemma.id);
            VM.LemmaList[index] = editLemma;

            StreamWriter SW = new StreamWriter(path);
            foreach (Lemma lemma in VM.LemmaList) {
                SW.WriteLine(lemma.ToLine());
            }
            SW.Close();
        }

/*----------------------------------------------------------------------------*/

        private void DeleteLemma(int deleteID) {
            Lemma delLemma = VM.LemmaList.Find(l => l.id == deleteID);
            VM.LemmaList.Remove(delLemma);
            }

/*----------------------------------------------------------------------------*/

        private int findMinID() {
            int min = 0;
            for (int i = 0; i <= VM.lemmaIDs.Count(); i++) {
                if (!VM.lemmaIDs.Contains(i)) {
                    min = i;
                    break;
                }
            }
            return min;
        }

/*----------------------------------------------------------------------------*/

        private void GenSentenceList(Language lang) {
            if (!System.IO.File.Exists(Path.GetFullPath($"Files/SentenceLists/{lang.ToString()}.csv")))
            {
                StreamReader SR = new StreamReader(Path.GetFullPath("Files/sentences.csv"));
                StreamWriter SW = new StreamWriter(Path.GetFullPath($"Files/SentenceLists/{lang.ToString()}.csv"));
                string line = SR.ReadLine();
                while (line != null) {
                    if (lang.ToString() == line.Split('\t')[1]) { SW.WriteLine(line); }
                    line = SR.ReadLine();
                }
                SR.Close();
                SW.Close();
            }
        }
    }
}
