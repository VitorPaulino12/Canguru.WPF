using System;
using System.Collections.Generic;
using System.Linq;
using Canguru.Core;

namespace Canguru.Business
{
    public static class RecuperacaoSenhaService
    {
        // Guarda os tokens em memória
        private static Dictionary<string, string> tokens = new Dictionary<string, string>();

        // 1. Gera e envia token
        public static string EnviarToken(string email)
        {
            var usuario = GerenciadorDeUsuarios.GetTodosUsuarios()
                                               .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (usuario == null)
                return null;

            string token = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
            tokens[email] = token;

            // Aqui você enviaria o email real, mas no protótipo retorna o token
            return token;
        }

        // 2. Valida token
        public static bool ValidarToken(string email, string token)
        {
            if (!tokens.ContainsKey(email))
                return false;

            return tokens[email] == token;
        }

        // 3. Troca a senha
        public static bool AlterarSenha(string email, string novaSenha)
        {
            var usuario = GerenciadorDeUsuarios.GetTodosUsuarios()
                                               .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (usuario == null)
                return false;

            usuario.Senha = novaSenha;

            // apaga token depois de usar
            if (tokens.ContainsKey(email))
                tokens.Remove(email);

            return true;
        }
    }
}
