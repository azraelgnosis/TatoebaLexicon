using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tatoeba.Models;

namespace Tatoeba.Pages
{
    public class LoginModel : PageModel
    {
        public User user { get; set; }

        public void OnGet(){
        }

        public IActionResult OnPost(User user) {
            if (Authenticate(user)) {
                return RedirectToAction("Index", "Home");
            } else {
                return RedirectToPage();
            }
        }

        private bool Authenticate(User user) {
            StreamReader SR = new StreamReader(Path.GetFullPath("Files/Users.csv"));
            string line = SR.ReadLine();
            while (line != null) {
                string[] arr = line.Split(',');
                if (user.username == arr[1] && user.password == arr[2]) { return true; }
                line = SR.ReadLine();
            }
            return false;
        }
    }
}
