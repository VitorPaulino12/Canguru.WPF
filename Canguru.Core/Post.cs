
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Post
    {
        public Usuario Autor { get; set; }
        public string Conteudo { get; set; }
        public DateTime Data { get; set; }

        // Adicione esta propriedade:
        public string Imagem { get; set; }

        public override string ToString()
        {
            return $"{Autor.Nome} - {Data:dd/MM/yyyy HH:mm}\n{Conteudo}";
        }
    }
}
