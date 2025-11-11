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

        public static bool CadastrarUsuario(Usuario novoUsuario)
        {
            if (Usuarios.Any(u => u.Login.Equals(novoUsuario.Login, StringComparison.OrdinalIgnoreCase)))
            {
 
                return false;
            }

            novoUsuario.Id = proximoId++;
            novoUsuario.DataEntrada = DateTime.Now;
            novoUsuario.Status = "Ativo";
            if (string.IsNullOrWhiteSpace(novoUsuario.Email))
            {
                novoUsuario.Email = $"{novoUsuario.Login}@canguru.com";
            }
            Usuarios.Add(novoUsuario);

            return true;
        }

        public static Usuario ValidarLogin(string login, string senha)
        {
            return Usuarios.FirstOrDefault(u => u.Login.ToLower() == login.ToLower() && u.Senha == senha);
        }

        public static List<Usuario> GetTodosUsuarios()
        {
            return Usuarios;
        }
        public static bool ExcluirUsuario(int id)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario != null)
            {
                Usuarios.Remove(usuario);
                return true;
            }

            return false;
        }
    }
}