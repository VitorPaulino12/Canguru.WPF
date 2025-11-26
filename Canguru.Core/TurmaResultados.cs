using System.Collections.Generic;
using System.Linq;

namespace Canguru.Core
{
    public class TurmaResultados
    {
        public List<ResultadoAluno> Resultados { get; private set; } = new();

        public void Adicionar(ResultadoAluno r)
        {
            Resultados.Add(r);
        }

        public double MediaSala()
        {
            return Resultados.Sum(x => x.Porcentagem) / Resultados.Count;
        }
    }
}
