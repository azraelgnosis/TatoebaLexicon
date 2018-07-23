using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tatoeba.Models
{
    public class ViewModel : IViewModel
    {
        public string user { get; set; }
        public Language currentLang { get; set; }
        public IEnumerable<SelectListItem> userLanguages { get; set; }

        public Lemma lemma { get; set; }
        public List<Lemma> LemmaList { get; set; }
        public List<int> lemmaIDs { get; set; }

        public string filter { get; set; }

        public List<string> langSentences { get; set; }
    }

    public interface IViewModel { }
}