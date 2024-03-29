using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Dive.App.Models
{
    public class User : IdentityUser<int>
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public int QuestionsCount { get; set; } = 0;

        public int CorrectAnswersCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Avatar(int size = 200)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(Email);
            byte[] hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2").ToLower());
            }

            return $"https://www.gravatar.com/avatar/{sb}?d=identicon&s={size}";
        }
    }
}