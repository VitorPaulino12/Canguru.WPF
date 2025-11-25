using System;
using System.Net;
using System.Net.Mail;

namespace Canguru.Business
{
    public static class EmailService
    {
        public static string EnviarOuSimular(string destinatario, string novaSenha)
        {
            try
            {
                // ====================================================================
                // PREENCHA AQUI SEUS DADOS REAIS DO GMAIL
                // ====================================================================
                string meuEmail = "seu.email.real@gmail.com";
                string minhaSenhaDeApp = "xxxx xxxx xxxx xxxx"; // Senha de App de 16 dígitos

                // Validação simples para não tentar conectar se você não configurou
                if (meuEmail.Contains("seu.email.real")) throw new Exception("Não configurado");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(meuEmail, "Suporte Canguru");
                mail.To.Add(destinatario);
                mail.Subject = "Recuperação de Senha";
                mail.Body = $"Olá!\n\nSua nova senha é: {novaSenha}\n\nUse-a para entrar no sistema.";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential(meuEmail, minhaSenhaDeApp);
                smtp.EnableSsl = true;

                smtp.Send(mail);

                return "OK"; // Sucesso real!
            }
            catch (Exception)
            {
                // Se falhar (ou se o usuário cadastrado tiver email falso "a@a.com"), 
                // devolve a senha para mostrar na tela.
                return novaSenha;
            }
        }
    }
}