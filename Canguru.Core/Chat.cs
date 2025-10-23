
using System;
using System.Collections.Generic;

namespace Canguru.Core
{
    public class Chat
    {
        private int proximoIdMensagem = 1;

        // CORREÇÃO: Os usuários do chat agora são do tipo 'Usuario'
        public Usuario Usuario1 { get; private set; }
        public Usuario Usuario2 { get; private set; }

        public List<Mensagem> Historico { get; private set; }

        // CORREÇÃO: O construtor agora aceita 'Usuario'
        public Chat(Usuario u1, Usuario u2)
        {
            // Garante uma ordem consistente para a chave do chat depois
            if (u1.Id < u2.Id)
            {
                Usuario1 = u1;
                Usuario2 = u2;
            }
            else
            {
                Usuario1 = u2;
                Usuario2 = u1;
            }

            Historico = new List<Mensagem>();
        }

        // CORREÇÃO: O método de enviar mensagem também aceita 'Usuario'
        public void EnviarMensagem(Usuario remetente, string conteudo)
        {
            if (remetente.Id != Usuario1.Id && remetente.Id != Usuario2.Id)
            {
                // Impede que alguém de fora do chat envie uma mensagem
                return;
            }

            Usuario destinatario = (remetente.Id == Usuario1.Id) ? Usuario2 : Usuario1;

            var novaMensagem = new Mensagem(remetente, destinatario, conteudo)
            {
                Id = proximoIdMensagem++
            };

            Historico.Add(novaMensagem);
        }
    }
}