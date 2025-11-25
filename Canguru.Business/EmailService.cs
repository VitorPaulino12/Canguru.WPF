using System;
using System.Net;
using System.Net.Mail;

namespace Canguru.Business
{
    public static class EmailService
    {
        /// <summary>
        /// Tenta enviar o email. Se falhar (modo simulação), retorna a senha gerada para ser exibida na tela.
        /// </summary>
        public static string EnviarOuSimular(string destinatario, string novaSenha)
        {
            try
            {
                // Configuração de exemplo (Preencher com dados reais se for usar SMTP de verdade)
                string meuEmail = "teste@canguru.com";
                string minhaSenha = "123";

                // Se os dados forem fictícios, forçamos o erro para cair no catch (Modo Simulação)
                if (meuEmail == "teste@canguru.com") throw new Exception("Modo Simulação");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(meuEmail);
                mail.To.Add(destinatario);
                mail.Subject = "Recuperação de Senha - Canguru";
                mail.Body = $"Olá! \n\nSua senha foi resetada.\nNova Senha: {novaSenha}";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(meuEmail, minhaSenha);
                smtp.EnableSsl = true;

                smtp.Send(mail);

                return "OK"; // Retorna OK se enviou de verdade
            }
            catch (Exception)
            {
                // Retorna a própria senha para a tela de Login exibir no MessageBox
                return novaSenha;
            }
        }
    }
}