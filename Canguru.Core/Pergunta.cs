using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Pergunta
    {
        public int Id { get; set; }

        // Estrutura de dados corrigida:
        public string Enunciado { get; set; }
        public List<string> Alternativas { get; set; }

        public int IndiceRespostaCorreta { get; set; } // ex: 0 para a primeira alternativa

        public Pergunta()
        {
            Alternativas = new List<string>();
        }

        public bool VerificarResposta(int indiceResposta)
        {
            return indiceResposta == IndiceRespostaCorreta;
        }
    }
}
