﻿using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using Usuarios.Application.Services.Interfaces;

namespace Usuarios.Application.Services
{
    public class SenhaHasher : ISenhaHasher
    {
        public string Hash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool Verify(string senha, string hash)
        {
            var hashDaSenha = Hash(senha);
            return hashDaSenha == hash;
        }


    }
}
