using System;
using System.Collections.Generic;

namespace Tatoeba.Models
{
    public class ViewModel : IViewModel
    {
        public string user { get; set; }
        public Language currentLang { get; set; }

        public Lemma lemma { get; set; }
        public List<Lemma> LemmaList { get; set; }
        public List<int> lemmaIDs { get; set; }
    }

    public interface IViewModel { }
}