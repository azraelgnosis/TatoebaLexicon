using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tatoeba.Models
{
    public class User
    {
        public int userID { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [NotMapped]
        [CompareAttribute("password")]
        public string confirmPassword { get; set; }

        public static User FromLine(string line) {
            string[] arr = line.Split(',');
            User user = new User
            {
                userID = int.Parse(arr[0]),
                username = arr[1],
                password = arr[2]
            };
            return user;
        }

        public User FromLine2(string line) {
            string[] arr = line.Split(',');
            User user = new User
            {
                userID = int.Parse(arr[0]),
                username = arr[1],
                password = arr[2]
            };
            return user;
        }

        public string ToLine() {
            string idStr = userID.ToString();
            string[] arr = { idStr, username, password };
            return String.Join(',', arr);
        }
    }
}
