using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication.Infrastructure.Extensions
{
    public static class HashPasswordExtensions
    {
        public static string Hash(this string Password)
        {
            var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }
    }
}