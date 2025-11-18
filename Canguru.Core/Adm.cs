using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Adm : Usuario
    {
        public Adm()
        {
            Tipo = TipoUsuario.Adm;
        }

        public override string ToString()
        {
            return $"[ADM] {Id} - {Nome} ({Email})";
        }
    }
}
