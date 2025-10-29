using System;

namespace Canguru.Core
{
    public class Post
    {
        public Usuario Autor { get; set; }
        public string Conteudo { get; set; }
        public DateTime Data { get; set; }
        public string Imagem { get; set; } // Caminho para a imagem do post

        public override string ToString()
        {
            return $"{Autor.Nome} - {Data:dd/MM/yyyy HH:mm}\n{Conteudo}";
        }
    }
}