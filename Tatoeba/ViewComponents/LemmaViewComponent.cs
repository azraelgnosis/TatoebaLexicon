using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Tatoeba.Models;

namespace Tatoeba.ViewComponents
{
    public class LemmaViewComponent : ViewComponent
    {
        ViewModel viewModel;

        public LemmaViewComponent(IViewModel iViewModel) {
            viewModel = (ViewModel)iViewModel;
        }

        public IViewComponentResult Invoke() {
            return View("LemmaList");
        }


    }
}
