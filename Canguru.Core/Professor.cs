
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Professor : Usuario // Herda de Usuario
    {
        // Exemplo de propriedade específica do professor
        public List<Quiz> QuizzesCriados { get; set; }

        public Professor()
        {
            Tipo = TipoUsuario.Professor; // Define o tipo automaticamente
            QuizzesCriados = new List<Quiz>();
        }
    }
}
