using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        private static List<Usuario> Usuarios { get; set; } = new List<Usuario>();
        private static int proximoId = 1;

        static GerenciadorDeUsuarios()
        {
            // Opcional: Adiciona um usuário admin para testes
            Professor admin = new Professor
            {
                Id = proximoId++,
                Nome = "Administrador",
                Email = "admin@canguru.com",
                Login = "admin",
                Senha = "123"
            };
            Usuarios.Add(admin);
        }

        public static void CadastrarUsuario(Usuario novoUsuario)
        {
            if (Usuarios.Any(u => u.Login.ToLower() == novoUsuario.Login.ToLower()))
            {
                throw new Exception("Este login já está em uso.");
            }
            novoUsuario.Id = proximoId++;
            Usuarios.Add(novoUsuario);
        }

        public static Usuario ValidarLogin(string login, string senha)
        {
            return Usuarios.FirstOrDefault(u => u.Login.ToLower() == login.ToLower() && u.Senha == senha);
        }

        public static List<Usuario> GetTodosUsuarios()
        {
            return Usuarios;
        }
    }
}