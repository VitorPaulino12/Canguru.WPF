using System;
using System.Collections.Generic;
using System.Linq;
using Canguru.Core;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        // --- BANCO DE DADOS EM MEMÓRIA ---
        private static List<Usuario> _bancoDeUsuarios = new List<Usuario>();

        static GerenciadorDeUsuarios()
        {
            // --- AQUI ESTÁ O ADMIN QUE VOCÊ PEDIU ---
            _bancoDeUsuarios.Add(new Adm
            {
                Id = 1,
                Nome = "Administrador",
                Login = "admin",   // Login
                Senha = "admin",   // Senha
                RA = "0000"
            });
        }

        // --- MÉTODOS DE LEITURA ---
        public static List<Usuario> GetTodosUsuarios()
        {
            return _bancoDeUsuarios;
        }

        public static Usuario ValidarLogin(string login, string senha)
        {
            // Busca usuário onde login e senha batem
            return _bancoDeUsuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
        }

        public static Usuario BuscarPorEmail(string email)
        {
            // Busca para recuperar senha (ignora maiúsculas/minúsculas)
            return _bancoDeUsuarios.FirstOrDefault(u => u.Login.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // --- MÉTODOS DE GRAVAÇÃO ---
        public static bool CadastrarUsuario(string nome, string login, string senha, string ra, string caminhoFotoPerfil)
        {
            // Verifica duplicidade de login
            if (_bancoDeUsuarios.Any(u => u.Login == login)) return false;

            Usuario novoUsuario;

            if (ra.StartsWith("1")) novoUsuario = new Aluno();
            else if (ra.StartsWith("2")) novoUsuario = new Professor();
            else return false;

            // Gera ID automático (pega o maior ID e soma 1)
            int novoId = _bancoDeUsuarios.Count > 0 ? _bancoDeUsuarios.Max(u => u.Id) + 1 : 1;

            novoUsuario.Id = novoId;
            novoUsuario.Nome = nome;
            novoUsuario.Login = login;
            novoUsuario.Senha = senha;
            novoUsuario.RA = ra;
            novoUsuario.CaminhoFotoPerfil = caminhoFotoPerfil;

            _bancoDeUsuarios.Add(novoUsuario);
            return true;
        }

        public static void AtualizarSenha(int idUsuario, string novaSenha)
        {
            var usuario = _bancoDeUsuarios.FirstOrDefault(u => u.Id == idUsuario);
            if (usuario != null)
            {
                usuario.Senha = novaSenha;
            }
        }

        public static bool ExcluirUsuario(int idUsuario)
        {
            Usuario usuarioParaRemover = _bancoDeUsuarios.FirstOrDefault(u => u.Id == idUsuario);

            if (usuarioParaRemover != null)
            {
                _bancoDeUsuarios.Remove(usuarioParaRemover);
                return true;
            }
            return false;
        }
    }
}