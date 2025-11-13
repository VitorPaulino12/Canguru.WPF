using System.Collections.Generic;

namespace Canguru.Core
{
    public class Aluno : Usuario 
    {
        

        public Aluno()
        {
            Tipo = TipoUsuario.Aluno; 
            
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} ({Email})";
        }
    }
}
