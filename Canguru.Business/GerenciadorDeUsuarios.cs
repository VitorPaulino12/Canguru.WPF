using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        private static List<Usuario> listaUsuarios = new List<Usuario>();


        // =====================================================================
        // CADASTRO (LoginWindow e CadastroWindow usam este método)
        // =====================================================================
        public static bool CadastrarUsuario(string nome, string login, string senha)
        {
            // Verifica se login já existe
            if (listaUsuarios.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                return false;

            // Gera ID incremental
            int novoId = listaUsuarios.Count == 0 ? 1 : listaUsuarios.Max(u => u.Id) + 1;

            // Aqui estava o erro:
            // ❌ new Usuario()  -> NÃO PODE, Usuario é abstract
            // ✔ new Professor()
            var novoUsuario = new Professor
            {
                Id = novoId,
                Nome = nome,
                Login = login,
                Senha = senha,
                Email = login,
                Status = "Ativo",
                DataEntrada = DateTime.Now
            };

            listaUsuarios.Add(novoUsuario);
            return true;
        }


        // =====================================================================
        // LOGIN
        // =====================================================================
        public static Usuario ValidarLogin(string login, string senha)
        {
            return listaUsuarios.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Senha == senha
            );
        }


        // =====================================================================
        // LISTAGEM
        // =====================================================================
        public static List<Usuario> GetTodosUsuarios()
        {
            return listaUsuarios;
        }


        public static Usuario GetUsuarioPorId(int id)
        {
            return listaUsuarios.FirstOrDefault(u => u.Id == id);
        }


        // =====================================================================
        // ATUALIZAR
        // =====================================================================
        public static bool AtualizarUsuario(Usuario usuarioAtualizado)
        {
            var usuario = listaUsuarios.FirstOrDefault(u => u.Id == usuarioAtualizado.Id);
            if (usuario == null)
                return false;

            usuario.Nome = usuarioAtualizado.Nome;
            usuario.Email = usuarioAtualizado.Email;
            usuario.Senha = usuarioAtualizado.Senha;
            usuario.Status = usuarioAtualizado.Status;
            usuario.CaminhoFotoPerfil = usuarioAtualizado.CaminhoFotoPerfil;

            return true;
        }


        // =====================================================================
        // EXCLUIR
        // =====================================================================
        public static bool ExcluirUsuario(int id)
        {
            var usuario = listaUsuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return false;

            listaUsuarios.Remove(usuario);
            return true;
        }
    }
}

