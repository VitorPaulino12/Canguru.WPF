using QuizTeste.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Sessao
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        // Trocou o array fixo por uma lista flexível
        public List<Pergunta> Perguntas { get; set; }

        public Sessao()
        {
            Perguntas = new List<Pergunta>();
        }
    }
}