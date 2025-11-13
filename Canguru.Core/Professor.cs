using System;

namespace Canguru.Core
{
    public class Professor : Usuario
    {
          public Professor()
        {
            Tipo = TipoUsuario.Professor;
            CaminhoFotoPerfil = @"C:\Canguru\Fotos\PadraoProfessor.png";
            DataEntrada = DateTime.Now;
            Status = "Ativo";
        }

        public override string ToString()
        {
            return $"{Id} - {Nome} ({Email})";
        }
    }
}
