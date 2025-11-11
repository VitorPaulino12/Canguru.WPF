using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Interacao
        
    {
        //Essa classe serve para guardar as informações de interações de usuários com o Quiz
        //para que dessa forma o professor tenha acesso a pergunta que ele interagiu, se a interação
        //foi um sucesso ou não e qual sessão essa pergunta pertence! porq isso pode revelar
        //um padrão.
        //os atributos estavam como privados mas eu n conseguia acessar o id da interacao no gerenciador
        //então eu o removi
        public int idInteracao { get; set; } //  para ser encontrada dpois no repositório
        public int idAluno { get; set; } //  guarda o id do aluno que a fez
        public int idPerguntaInteracao { get; set; }//guarda a pergunta que foi respondida
        public int resultadoInteração { get; set; }// guarda se foi um acerto ou um erro
        public DateTime DataInteracao { get; set; } = DateTime.Now;
        public void getInteracao()
        {
        //Retorna o id da interação
        }

    }
}
