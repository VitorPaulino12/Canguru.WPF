using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Business
{
     public class GerenciadorInteracao
    {
        private static List<Interacao> historico_Global = new List<Interacao>();

        public static void AddInteracao(int idAluno, int idPerguntaInteracao, int resultadoInteracao)
        {
            // Gera automaticamente o próximo ID
            int novoId = historico_Global.Count == 0 ? 1 : historico_Global.Max(i => i.idInteracao) + 1;

            Interacao novaInteracao = new Interacao
            {
                idInteracao = novoId,
                idAluno = idAluno,
                idPerguntaInteracao = idPerguntaInteracao,
                resultadoInteração = resultadoInteracao,
                DataInteracao = DateTime.Now
            };

            historico_Global.Add(novaInteracao);
        }

        public static List<Interacao> GetInteracoes()
        {
            return historico_Global;
        }  
        }

}

