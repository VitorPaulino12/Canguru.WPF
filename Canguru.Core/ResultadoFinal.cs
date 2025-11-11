using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class ResultadoFinal
    {
              //IdentificadorQuiz =novoid,
             //   idAluno = IdAluno,
             //   MediaTotal = MediaTotal

        public int IdentificadorQuiz { get; set; }
        public int idAluno { get; set; }
        public double MediaTotal { get; set; }
        public DateTime DataEntradaResult { get; set; } = DateTime.Now;
    }
}
