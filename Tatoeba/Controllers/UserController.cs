using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tatoeba.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tatoeba.Controllers
{
    public class UserController : Controller
    {
        string usersPath = Path.GetFullPath("Files/Users.csv");
        List<int> userIDs;

        public UserController() {}

/*----------------------------------------------------------------------------*/

        [HttpGet]
        public IActionResult Login() { return View(); }

/*----------------------------------------------------------------------------*/

        [HttpPost]
        public IActionResult Login(User user) {
            if (Authenticate(user)) { return RedirectToAction("Index", "Home", new { name = user.username}); }
            return RedirectToAction("Login", new { id = -1 });
        }

/*----------------------------------------------------------------------------*/

        [HttpPost]
        public IActionResult NewUser(User newUser) {
            if (UserExists(newUser)) { return RedirectToAction("Login", new { id = -2 }); }
            if (String.IsNullOrEmpty(newUser.password)) { return RedirectToAction("Login", new { id = -3 }); }
            if (!newUser.password.Equals(newUser.confirmPassword)) { return RedirectToAction("Login", new { id = -4 }); }

            AddUser(newUser);
            System.IO.File.Create(Path.GetFullPath("Files/Users") + $"/{newUser.username}.csv");
            return RedirectToAction("Login", new { id = 2 });
        }

/*----------------------------------------------------------------------------*/
/*----------------------------------------------------------------------------*/
// Authenticate
        private bool Authenticate(User user)
        {
            StreamReader SR = new StreamReader(usersPath);
            string line = SR.ReadLine();
            while (line != null)
            {
                string[] arr = line.Split(',');
                if (user.username == arr[1] && user.password == arr[2]) { return true; }
                line = SR.ReadLine();
            }
            return false;
        }

/*----------------------------------------------------------------------------*/

        private void AddUser(User newUser) {
            GetUserIds();
            newUser.userID = FindMinID();

            StreamWriter SW = new StreamWriter(usersPath, true);
            SW.WriteLine(newUser.ToLine());
            SW.Close();
        }

/*----------------------------------------------------------------------------*/

        private bool UserExists(User user) {
            StreamReader SR = new StreamReader(usersPath);
            string line = SR.ReadLine();
            while (line != null) {
                if (line.Split(',')[1] == user.username) {
                    return true;
                }
                line = SR.ReadLine();
            }
            return false;
        }

/*----------------------------------------------------------------------------*/

        private void GetUserIds() {
            userIDs = new List<int>();
            StreamReader SR = new StreamReader(usersPath);
            string line = SR.ReadLine();

            while (line != null) {
                userIDs.Add(int.Parse(line.Split(',')[0]));
                line = SR.ReadLine();
            }
            SR.Close();
        }

/*----------------------------------------------------------------------------*/

        private int FindMinID() {
            for (int i = 0; i <= userIDs.Count(); i++) {
                if (!userIDs.Contains(i)) {
                    return i;
                }
            }
            return userIDs.Count();
        }

/*----------------------------------------------------------------------------*/

    }
}
