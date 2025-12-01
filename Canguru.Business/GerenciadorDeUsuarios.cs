using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        private static List<Usuario> _bancoDeUsuarios = new List<Usuario>();

        static GerenciadorDeUsuarios()
        {
            // 1. Criado como PROFESSOR
            // 2. RA começa com '2'
            _bancoDeUsuarios.Add(new Professor
            {
                Id = 999, 
                Nome = "Administrador Sistema",
                Login = "admin",
                Email = "admin@canguru.com",
                Senha = Criptografia.GerarHash("123"),
                RA = "20000000000", // COMEÇA COM 2 = PROFESSOR
                CaminhoFotoPerfil = null
            });
        }

        public static List<Usuario> GetTodosUsuarios()
        {
            return _bancoDeUsuarios;
        }

        public static Usuario ValidarLogin(string login, string senhaDigitada)
        {
            string senhaCriptografada = Criptografia.GerarHash(senhaDigitada);

            return _bancoDeUsuarios.FirstOrDefault(u =>
                (u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) ||
                 (u.Email != null && u.Email.Equals(login, StringComparison.OrdinalIgnoreCase))) &&
                u.Senha == senhaCriptografada);
        }

        public static bool CadastrarUsuario(string nome, string login, string senha, string ra, string caminhoFotoPerfil)
        {
            if (_bancoDeUsuarios.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                return false;

            Usuario novoUsuario;

            if (ra.StartsWith("1"))
            {
                novoUsuario = new Aluno(); // Começa com 1 = Aluno
            }
            else if (ra.StartsWith("2"))
            {
                novoUsuario = new Professor(); // Começa com 2 = Professor
            }
            else
            {
                return false; // RA Inválido (não começa com 1 nem 2)
            }

            int novoId = _bancoDeUsuarios.Count > 0 ? _bancoDeUsuarios.Max(u => u.Id) + 1 : 1;

            novoUsuario.Id = novoId;
            novoUsuario.Nome = nome;
            novoUsuario.Login = login;
            novoUsuario.Email = login;
            novoUsuario.Senha = Criptografia.GerarHash(senha);
            novoUsuario.RA = ra; 
            novoUsuario.CaminhoFotoPerfil = caminhoFotoPerfil;

            _bancoDeUsuarios.Add(novoUsuario);
            return true;
        }

        // Métodos
        public static Usuario BuscarPorEmail(string email) => _bancoDeUsuarios.FirstOrDefault(u => u.Login.Equals(email, StringComparison.OrdinalIgnoreCase));
        public static void AtualizarSenha(int id, string s) { var u = _bancoDeUsuarios.FirstOrDefault(x => x.Id == id); if (u != null) u.Senha = Criptografia.GerarHash(s); }
        public static bool ExcluirUsuario(int id) { var u = _bancoDeUsuarios.FirstOrDefault(x => x.Id == id); if (u != null) { _bancoDeUsuarios.Remove(u); return true; } return false; }
    }
}