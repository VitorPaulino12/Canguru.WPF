using System.Security.Cryptography;
using System.Text;

namespace Canguru.Business
{
    public static class Criptografia
    {
        public static string GerarHash(string senha)
        {
            // Cria a ferramenta de hash SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Converte a senha para bytes
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));

                // Converte os bytes de volta para uma string legível (Hexadecimal)
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}