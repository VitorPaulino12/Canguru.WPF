using Canguru.Core;

namespace Canguru.Business
{
    public static class GerenciadorDeUsuarios
    {
        private static List<Usuario> _bancoDeUsuarios = new List<Usuario>();

        static GerenciadorDeUsuarios()
        {
            // --- ATENÇÃO: AGORA A SENHA DO ADMIN TAMBÉM É CRIPTOGRAFADA ---
            _bancoDeUsuarios.Add(new Adm
            {
                Id = 1,
                Nome = "Administrador Sistema",
                Login = "admin",
                // AQUI: Transformamos "admin" em código secreto antes de salvar
                Senha = Criptografia.GerarHash("123"),
                RA = "0000"
            });
        }

        public static List<Usuario> GetTodosUsuarios()
        {
            return _bancoDeUsuarios;
        }

        public static Usuario ValidarLogin(string login, string senhaDigitada)
        {
            // 1. Criptografa o que o usuário digitou agora
            string senhaCriptografada = Criptografia.GerarHash(senhaDigitada);

            // 2. Compara o LOGIN e a SENHA CRIPTOGRAFADA
            return _bancoDeUsuarios.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Senha == senhaCriptografada);
        }

        public static Usuario BuscarPorEmail(string email)
        {
            return _bancoDeUsuarios.FirstOrDefault(u => u.Login.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public static bool CadastrarUsuario(string nome, string login, string senha, string ra, string caminhoFotoPerfil)
        {
            if (_bancoDeUsuarios.Any(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                return false;

            Usuario novoUsuario;

            if (ra.StartsWith("1")) novoUsuario = new Aluno();
            else if (ra.StartsWith("2")) novoUsuario = new Professor();
            else return false;

            int novoId = _bancoDeUsuarios.Count > 0 ? _bancoDeUsuarios.Max(u => u.Id) + 1 : 1;

            novoUsuario.Id = novoId;
            novoUsuario.Nome = nome;
            novoUsuario.Login = login;

            // AQUI: Criptografamos a senha antes de salvar na lista!
            novoUsuario.Senha = Criptografia.GerarHash(senha);

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
                // AQUI: Se resetar a senha, salvamos a nova versão criptografada
                usuario.Senha = Criptografia.GerarHash(novaSenha);
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