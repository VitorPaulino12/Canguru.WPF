using System.Collections.Generic;

namespace Canguru.Core
{
    public class Aluno : Usuario // Herda de Usuario
    {
        

        public Aluno()
        {
            Tipo = TipoUsuario.Aluno; // Define o tipo automaticamente
            
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} ({Email})";
        }
    }
}
