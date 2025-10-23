using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canguru.Core
{
    public class Quiz : Atividade
    {
        public List<Sessao> Sessoes { get; set; }

        public Quiz(int id, string nome, string descricao, int idCriador)
            : base(id, nome, descricao, idCriador)
        {
            Sessoes = new List<Sessao>();
        }

        public void AdicionarSessao(Sessao sessao)
        {
            Sessoes.Add(sessao);
        }

        public override void ExibirDetalhes()
        {
            Console.WriteLine($"Quiz: {Nome} (ID: {Id})");
            Console.WriteLine($"Descrição: {Descricao}");
            Console.WriteLine($"Criado por: {IdCriador}");
            Console.WriteLine($"Total de Sessões: {Sessoes.Count}");
        }
    }
}
