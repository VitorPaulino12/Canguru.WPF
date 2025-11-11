using Canguru.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Business
{
    public class GerenciadorResultFinal
    {
        private static List<ResultadoFinal> Historico_GlobalResultados = new List<ResultadoFinal>();
        //private static List<Interacao> historico_Global = new List<Interacao>();
        public static void addResultado_Lista(int IdAluno, double MediaTotal1)
        {
            //gerando um novo id
            int novoid = Historico_GlobalResultados.Count == 0 ? 1 : Historico_GlobalResultados.Max(i => i.IdentificadorQuiz) + 1;

            ResultadoFinal AcrecimoNAlista = new ResultadoFinal
            {
                IdentificadorQuiz =novoid,
                idAluno = IdAluno,
                MediaTotal = MediaTotal1,
                DataEntradaResult = DateTime.Now

            };
            Historico_GlobalResultados.Add(AcrecimoNAlista);
        }
        public static List<ResultadoFinal> GetResultados()
        {
            return Historico_GlobalResultados;
        }
    }
}
