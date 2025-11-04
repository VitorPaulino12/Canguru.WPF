using System;

namespace Canguru.Core
{
    public class Post
    {
        public Usuario Autor { get; set; }
        public string Conteudo { get; set; }
        public DateTime Data { get; set; }
        

        public override string ToString()
        {
            return $"{Autor.Nome} - {Data:dd/MM/yyyy HH:mm}\n{Conteudo}";
        }
    }
}