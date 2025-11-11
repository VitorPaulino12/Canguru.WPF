using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public abstract class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; }
        public string Senha { get; set; }
        public TipoUsuario Tipo { get; set; }
        public string Imagem { get; set; }
        public string CaminhoFotoPerfil { get; set; }
        public DateTime DataEntrada { get; set; } = DateTime.Now; // Adicionei para colocar a data de entrda de um adastro no sistema
        public string Status { get; set; } = "Ativo";// seria bom se tivesse um trecho de código que permite trocar para 'Inativo' ou algo assim, mas 
                                                        //parace que pra um protótipo n faz sentido colocar isso (se o usuario n entraq a 30 dias ou algo assim)
    }
}
