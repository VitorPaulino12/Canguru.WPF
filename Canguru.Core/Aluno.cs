using System.Collections.Generic;

namespace Canguru.Core
{
    public class Aluno : Usuario // Herda de Usuario
    {
        public LinkedList<Mensagem> MensagensEnviadas { get; set; }

        public Aluno()
        {
            Tipo = TipoUsuario.Aluno; // Define o tipo automaticamente
            MensagensEnviadas = new LinkedList<Mensagem>();
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} ({Email})";
        }
    }
}
