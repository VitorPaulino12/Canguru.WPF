
using System;
using System.Collections.Generic;

namespace Canguru.Core
{
    public class Mensagem
    {
        public int Id { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataHora { get; set; }

        // CORREÇÃO: Remetente e Destinatário agora são do tipo 'Usuario'
        public Usuario Remetente { get; set; }
        public Usuario Destinatario { get; set; }

        // O construtor é opcional, mas se tiver um, ele também deve ser atualizado.
        public Mensagem(Usuario remetente, Usuario destinatario, string conteudo)
        {
            this.Remetente = remetente;
            this.Destinatario = destinatario;
            this.Conteudo = conteudo;
            this.DataHora = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{DataHora:HH:mm} {Remetente.Nome}: {Conteudo}";
        }
    }
}
