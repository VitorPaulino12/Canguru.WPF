using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        // Continuamos usando a lista, como estava originalmente
        private static List<Usuario> listaUsuarios = new List<Usuario>();

        // Construtor estático para criar o usuário ADM na inicialização
        static GerenciadorDeUsuarios()
        {
            // Cria o usuário Administrador e o adiciona à lista
            var usuarioAdm = new Adm
            {
                Id = 0, // ID fixo para o ADM
                Nome = "Administrador do Sistema",
                Login = "adm",
                Senha = "123",
                Email = "adm@sistema.com",
                Status = "Ativo",
                DataEntrada = DateTime.Now
            };
            listaUsuarios.Add(usuarioAdm);
        }
        public static bool CadastrarUsuario(string nome, string login, string senha, bool isProfessor, string caminhoFotoPerfil = null)
        {
            if (listaUsuarios.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                return false;

            // Gera ID incremental baseado no maior ID já existente na lista
            int novoId = listaUsuarios.Count == 0 ? 1 : listaUsuarios.Max(u => u.Id) + 1;

            // Cria o objeto correto (Aluno ou Professor) baseado no booleano
            Usuario novoUsuario;
            if (isProfessor)
            {
                novoUsuario = new Professor();
            }
            else
            {
                novoUsuario = new Aluno();
            }

            novoUsuario.Id = novoId;
            novoUsuario.Nome = nome;
            novoUsuario.Login = login;
            novoUsuario.Senha = senha;
            novoUsuario.Email = login;
            novoUsuario.Status = "Ativo";
            novoUsuario.DataEntrada = DateTime.Now;
            novoUsuario.CaminhoFotoPerfil = caminhoFotoPerfil ?? @"\assets\img\default.png";

            listaUsuarios.Add(novoUsuario);
            return true;
        }
        public static Usuario ValidarLogin(string login, string senha)
        {
            return listaUsuarios.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Senha == senha
            );
        }
        public static List<Usuario> GetTodosUsuarios()
        {
            return listaUsuarios;
        }

        public static Usuario GetUsuarioPorId(int id)
        {
            return listaUsuarios.FirstOrDefault(u => u.Id == id);
        }

        public static bool AtualizarUsuario(Usuario usuarioAtualizado)
        {
            var usuario = listaUsuarios.FirstOrDefault(u => u.Id == usuarioAtualizado.Id);
            if (usuario == null) return false;

            usuario.Nome = usuarioAtualizado.Nome;
            usuario.Email = usuarioAtualizado.Email;
            usuario.Senha = usuarioAtualizado.Senha;
            usuario.Status = usuarioAtualizado.Status;
            usuario.CaminhoFotoPerfil = usuarioAtualizado.CaminhoFotoPerfil;

            return true;
        }

        public static bool ExcluirUsuario(int id)
        {
            var usuario = listaUsuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null) return false;

            listaUsuarios.Remove(usuario);
            return true;
        }
    }
}