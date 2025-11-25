using System;
using System.Collections.Generic;
using System.Linq;
using Canguru.Core;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        // --- BANCO DE DADOS COMPARTILHADO (MEMÓRIA RAM) ---
        // Todos os usuários (Admin e Cadastrados na hora) ficam aqui.
        private static List<Usuario> _bancoDeUsuarios = new List<Usuario>();

        static GerenciadorDeUsuarios()
        {
            // Usuário padrão para você não ficar trancado fora do sistema
            _bancoDeUsuarios.Add(new Adm
            {
                Id = 1,
                Nome = "Administrador",
                Login = "admin",  // Você pode trocar por um email se preferir
                Senha = "123",
                RA = "0000"
            });
        }

        // --- MÉTODOS DE BUSCA ---
        public static List<Usuario> GetTodosUsuarios()
        {
            return _bancoDeUsuarios;
        }

        public static Usuario ValidarLogin(string login, string senha)
        {
            // Verifica Login e Senha exatos
            return _bancoDeUsuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
        }

        public static Usuario BuscarPorEmail(string email)
        {
            // Busca pelo Login/Email (Ignora maiúsculas/minúsculas para facilitar)
            // Funciona tanto para "admin" quanto para "joao@gmail.com"
            return _bancoDeUsuarios.FirstOrDefault(u => u.Login.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        // --- MÉTODOS DE AÇÃO ---
        public static bool CadastrarUsuario(string nome, string login, string senha, string ra, string caminhoFotoPerfil)
        {
            // Evita duplicidade
            if (_bancoDeUsuarios.Any(u => u.Login == login)) return false;

            Usuario novoUsuario;

            // Define o tipo
            if (ra.StartsWith("1")) novoUsuario = new Aluno();
            else if (ra.StartsWith("2")) novoUsuario = new Professor();
            else return false;

            // Gera ID novo (Auto Incremento Simulado)
            int novoId = _bancoDeUsuarios.Count > 0 ? _bancoDeUsuarios.Max(u => u.Id) + 1 : 1;

            novoUsuario.Id = novoId;
            novoUsuario.Nome = nome;
            novoUsuario.Login = login; // O Login É O EMAIL cadastrado
            novoUsuario.Senha = senha;
            novoUsuario.RA = ra;
            novoUsuario.CaminhoFotoPerfil = caminhoFotoPerfil;

            // ADICIONA NA LISTA GERAL
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